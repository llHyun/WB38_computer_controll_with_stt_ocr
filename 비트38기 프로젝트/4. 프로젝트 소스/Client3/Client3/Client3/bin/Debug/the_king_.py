import pyaudio
import wave
import threading
import queue
import time
import numpy as np
import os

LOCK_FILE = "python_script.lock"  # 잠금 파일의 이름

#데시벨 측정해 녹음 시작을 설정할 수 있는 클래스
class VoiceActivityDetector:
    def __init__(self, threshold_db=68, min_silence_duration=1):
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

#데시벨 계산 함수
def calculate_decibel(chunk):
    data = np.frombuffer(chunk, dtype=np.int16)
    rms = np.sqrt(np.mean(data.astype(np.float32)**2))
    decibel = 20 * np.log10(rms)
    return decibel

#WAV저장하는 클래스
#recorder.start_recording()의 선언을 통해서 시작
#해당 함수는 데시벨이 설정 수치를 넘어가기 전까지 무한루프
#수치가 넘어가기시작하면 녹음을 시작하고, 녹음 함수인 recording_thread()를 시작시킴
#그러다가 측정 데시벨 이상의 소리가 지정한 시간보다 길게 들리지 않으면
#self.is_recording에게 false를 주어 recording 무한루프를 멈추고,
#파일을 WAV로 저장시킴 그 후 모든 과정이 끝나면 다시 start_recording을 실행시켜
#녹음 대기
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
            self.save_to_wav("C:\\Temp\\client\\record\\lastrec.wav")
            self.frames = []
            self.recording_thread = None
            self.start_time = None
            self.start_recording()

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
    if os.path.isfile(LOCK_FILE):
        print("Another instance is already running. Exiting.")
        exit()
    
    with open(LOCK_FILE, "w") as lock_file:
        lock_file.write("LOCK")

    recorder = AudioRecorder()
    recorder.start_recording()

    try:
        while True:
            time.sleep(1)
    except KeyboardInterrupt:
        print("Ctrl+C input detected. Stopping recording and saving files...")