import pyaudio
import wave
import threading
import queue
import time
import numpy as np

#같은 폴더 안에 있는 the_king_.py의 주석을 먼저 보고 오면 좋다.
#완전 똑같은 코드
class VoiceActivityDetector:
    def __init__(self, threshold_db=60, min_silence_duration=1):
        self.threshold_db = threshold_db
        self.min_silence_duration = min_silence_duration
        self.silence_start_time = None

    def is_speech(self, data):
        decibel = calculate_decibel(data)

        if decibel > self.threshold_db:
            self.silence_start_time = None
            return 0
        else:
            if self.silence_start_time is None:
                self.silence_start_time = time.time()
            else:
                silence_duration = time.time() - self.silence_start_time
                if silence_duration >= self.min_silence_duration:
                    return 2
            return 1

#다른 부분 -> 미리 빈 WAV파일을 만드는 코드(클라이언트에게 이벤트를 주기 위함)
#중요*****************
#해당 이벤트는 총 3번 발생됨 

#1. 이 빈 WAV 파일이 생성될때
#2. 이 WAV파일에 덮어쓰기를 하기 위해 정보를 덮어쓰기 할때(이름 위치 등등)
#3. 해당 파일에 데이터를 덮어쓰기 할때
#이 3가지 이벤트를 통해 녹음하는 과정을 실시간으로 사용자에게 보여줄 수 있음
def create_empty_wav(file_name):
    with wave.open(file_name, 'w') as wav_file:
        wav_file.setnchannels(1)
        wav_file.setsampwidth(2)
        wav_file.setframerate(44100)
        wav_file.setnframes(0)

def calculate_decibel(chunk):
    data = np.frombuffer(chunk, dtype=np.int16)
    rms = np.sqrt(np.mean(data.astype(np.float32)**2))
    decibel = 20 * np.log10(rms)
    return decibel

class AudioRecorder:
    def __init__(self, chunk=1024, channels=1, rate=44100):
        self.chunk = chunk
        self.channels = channels
        self.rate = rate
        self.audio_format = pyaudio.paInt16
        self.audio_stream = None
        self.frames = []
        self.is_recording = False
        self.queue = queue.Queue()
        self.vad = VoiceActivityDetector()
        self.recording_thread = None
        self.start_time = None
        self.run_program = True
        self.audio_name_bytime = None

    def start_recording(self):
        self.audio_stream = pyaudio.PyAudio().open(
            format=self.audio_format,
            channels=self.channels,
            rate=self.rate,
            input=True,
            frames_per_buffer=self.chunk
        )

        while True:
            dec_data = self.audio_stream.read(self.chunk)
            vad_result = self.vad.is_speech(dec_data)

            if vad_result == 0:
                self.is_recording = True
                self.recording_thread = threading.Thread(target=self._record_audio)
                self.recording_thread.daemon = True
                self.audio_name_bytime = time.strftime('%Y%m%d%H%M%S')
                create_empty_wav(f"C:\\Temp\\client\\study_voice\\{self.audio_name_bytime}.wav")
                self.start_time = time.time()
                self.recording_thread.start()
                return

    def stop_recording(self):
        if self.is_recording:
            self.is_recording = False
            if self.start_time is not None:
                elapsed_time = time.time() - self.start_time
                print(f"Recording stopped. Elapsed time: {elapsed_time:.2f} seconds")
            self.audio_stream.stop_stream()
            self.audio_stream.close()
            self.audio_stream = None
            
            self.save_to_wav(f"C:\\Temp\\client\\study_voice\\{self.audio_name_bytime}.wav")
                     
            self.frames = []
            self.recording_thread = None
            self.start_time = None
            self.run_program = False

    def save_to_wav(self, filename):
        with wave.open(filename, 'wb') as wf:
            wf.setnchannels(self.channels)
            wf.setsampwidth(pyaudio.PyAudio().get_sample_size(self.audio_format))
            wf.setframerate(self.rate)
            wf.writeframes(b''.join(self.frames))

    def _record_audio(self):
        start_time = time.time()
        while self.is_recording:
            data = self.audio_stream.read(self.chunk)
            self.queue.put(data)
            self.frames.append(data)
            is_speech = self.vad.is_speech(np.frombuffer(data, dtype=np.int16))
            if is_speech == 2:
                self.stop_recording()
                break
            ref = False
            if is_speech == 0:
                ref = True
            else:
                ref = False
            if self.start_time is not None:
                elapsed_time = time.time() - start_time
                audio_length = len(data) / self.rate
                print(f"Is Speech: {ref}, Elapsed Time: {elapsed_time:.2f} seconds, Audio Length: {audio_length:.2f} seconds")

if __name__ == "__main__":
    recorder = AudioRecorder()
    recorder.start_recording()

    try:
        #단순 무한루프가 아닌 bool형 변수를 주어 녹음이 끝나면 반복이 종료되고 python 프로세서가 종료되도록 코딩
        while recorder.run_program:
            time.sleep(1)
    except KeyboardInterrupt:
        print("Ctrl+C input detected. Stopping recording and saving files...")