import tensorflow as tf
import numpy as np
import librosa
import time
from watchdog.observers import Observer
from watchdog.events import FileSystemEventHandler
import tensorflow as tf
import os
import sys
import wave

# 이벤트를 감지할 수 있는 함수가 정의된 class
class MyHandler(FileSystemEventHandler):
    def __init__(self):
        super().__init__()
        self.called_distract = False
        self.the_id = None

    def on_modified(self, event):
        if not event.is_directory:
            if self.called_distract:
                predict_model(self.the_id)
                self.called_distract = False
                return
            self.called_distract = True

def predict_model(id):
    # 새로운 음성 파일 로드
    save_file_path = "C:\\Temp\\Server\\" + id + "\\result\\result.txt"
    test_audio_file = "C:\\Temp\\Server\\" + id + "\\record\\lastrec.wav"

    y, sr = librosa.load(test_audio_file)
    max_length = 2*22050

    if len(y) < max_length:
        y = np.pad(y, (0, max_length - len(y)))
    else:
        y = y[:max_length]

    # MFCC 추출
    mfccs = librosa.feature.mfcc(y=y, sr=sr, n_mfcc=13)

    # Z-점수 정규화
    # mfccs_mean = np.mean(mfccs, axis=1)
    # mfccs_std = np.std(mfccs, axis=1)
    # mfccs_normalized = (mfccs - mfccs_mean[:, np.newaxis]) / (mfccs_std[:, np.newaxis] + 1e-8)

    # 모델이 요구하는 입력 형태로 맞추기
    mfccs_padded = np.pad(mfccs, ((0, 0), (0, max_length - mfccs.shape[1])), mode='constant')

    input_data = np.expand_dims(mfccs_padded.T, axis=0)
    input_data = input_data[:, :44, :]  # 모델이 요구하는 형태로 자르기

    # 모델 불러오기
    model_directory = "C:\\Temp\\Server\\" + id
    model_name = id + "_model"
    model_path = os.path.join(model_directory, model_name + ".h5")
    loaded_model = tf.keras.models.load_model(model_path)

    # 모델 예측
    prediction = loaded_model.predict(input_data)

    # 예측 결과 출력
    probability = prediction[0][0]
    formatted_probability = "{:.2f}".format(probability * 100)
    print(f"{formatted_probability}%")

    # 기준치
    threshold = 0.6
    # 결과 출력
    if probability > threshold:
        content = "true"
    else:
        content = "false"
    with open(save_file_path, "w") as file:
        file.write(content)

# 모델에게 필요한 파일 미리 생성(안하면 터짐)
def file_create(id, rec_path):
    rec_file_name = "lastrec.wav"
    txt_path = "C:\\Temp\\Server\\" + id + "\\result"
    file_name = "result.txt"
    txt_full_path = os.path.join(txt_path, file_name)
    rec_full_path = os.path.join(rec_path, rec_file_name)
    if not os.path.exists(rec_path):
        os.makedirs(rec_path)

    if not os.path.exists(txt_path):
        os.makedirs(txt_path)
    with open(txt_full_path, 'w') as file:
        pass
    with wave.open(rec_full_path, 'w') as wav_file:
        wav_file.setparams((1, 2, 44100, 0, 'NONE', 'not compressed'))

if __name__ == "__main__":
    id = sys.argv[1]
    rec_path = "C:\\Temp\\Server\\" + id + "\\record"  # 모니터링할 디렉토리 경로
    file_create(id, rec_path)
    # 모니터링 할 수 있는 코드
    event_handler = MyHandler()
    observer = Observer()
    observer.schedule(event_handler, rec_path, recursive=False)
    observer.start()
    event_handler.the_id = id
    try:
        while True:
            time.sleep(1)
    except KeyboardInterrupt:
        observer.stop()
    observer.join()