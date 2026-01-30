import os
import sys
import librosa
import numpy as np
import tensorflow as tf
from sklearn.model_selection import train_test_split
from audiomentations import Compose, AddGaussianNoise, TimeStretch, PitchShift
from librosa.util import normalize

def study_Model(id):
    # 경고 메시지 숨기기
    os.environ['TF_CPP_MIN_LOG_LEVEL'] = '3'

    # 음성 파일 경로
    audio_folder_target_speaker = "C:\\Temp\\Server\\" + id + "\\study"
    audio_folder_other_speakers = "C:\\Users\\user\\Desktop\\보고서&발표자료\\최종 제출 서류\\4. 프로젝트 소스\\no_zara"

    # 모든 오디오 시그널을 동일한 길이로 자르기
    max_length = 22050*2  # 1초 길이의 오디오 시그널 

    # 화자 특징 추출
    speaker_audio_data = []
    speaker_labels = []

    # 호출어를 화자로 바꿔 화자 특징 추출
    for file in os.listdir(audio_folder_target_speaker)[:10]:
        y, sr = librosa.load(os.path.join(audio_folder_target_speaker, file)[:-4] + ".wav")

        # 호출어 감지된 부분 추출
        # (이 부분은 호출어 인식 모델로 처리된 후의 호출어가 감지된 음성 신호임을 가정합니다)
        # 호출어 인식 모델을 사용하여 호출어가 감지된 위치를 식별하고 해당 부분을 추출합니다.

        # 길이가 max_length보다 작으면 패딩, 그렇지 않으면 자르기
        if len(y) < max_length:
            y = np.pad(y, (0, max_length - len(y)))
        else:
            y = y[:max_length]

        # 오디오 데이터 정규화
        y = normalize(y)

        # MFCC 추출
        mfccs = librosa.feature.mfcc(y=y, sr=sr, n_mfcc=13, n_fft=min(2048, len(y)))
        speaker_audio_data.append(mfccs.T)
        # 화자에 대한 라벨 추가
        speaker_labels.append(1)  # 여기서 "speaker_id"는 호출어 감지된 화자의 고유 ID 또는 라벨입니다.

    # 다른 음성 또는 배경 소음 로드 및 특징 추출
    audio_data_other_samples = []
    labels_other_samples = []

    for file in os.listdir(audio_folder_other_speakers)[:30]:
        y, sr = librosa.load(os.path.join(audio_folder_other_speakers, file)[:-4] + ".wav")

        if len(y) < max_length:
            y = np.pad(y, (0, max_length - len(y)))
        else:
            y = y[:max_length]

        y = normalize(y)

        mfccs = librosa.feature.mfcc(y=y, sr=sr, n_mfcc=13)
        audio_data_other_samples.append(mfccs.T)
        labels_other_samples.append(0)

    # 훈련 세트에 마이크로폰 가변성 포함
    audio_data_total = speaker_audio_data + audio_data_other_samples
    labels_total = speaker_labels + labels_other_samples

    # 특징 벡터 및 레이블을 사용하여 모델 학습
    X = np.array(audio_data_total)
    y = np.array(labels_total)

    # 데이터를 학습 및 테스트 세트로 분할
    X_train_speaker, X_test_speaker, y_train_speaker, y_test_speaker = train_test_split(X, y, test_size=0.2, random_state=42)

    # 데이터 증강을 위한 Audiomentations 설정
    augment = Compose([
        AddGaussianNoise(min_amplitude=0.001, max_amplitude=0.01),
        TimeStretch(min_rate=0.8, max_rate=1.6),
        PitchShift(min_semitones=-2, max_semitones=2)
    ])

    # 데이터를 3D 배열로 변환
    X_train_speaker_combined_3d = np.array([normalize(augment(samples=x, sample_rate=22050)) for x in X_train_speaker])

    # 클래스 가중치 계산
    positive_class_ratio = np.sum(y_train_speaker == 1) / len(y_train_speaker)
    negative_class_ratio = np.sum(y_train_speaker == 0) / len(y_train_speaker)

    # 클래스 가중치 계산
    speaker_class_weights = {0: 1 / negative_class_ratio, 1: 3 / positive_class_ratio}

    # 각 화자 ID를 고유한 숫자로 매핑
    speaker_id_mapping = {speaker_id: i for i, speaker_id in enumerate(np.unique(speaker_labels))}


    # 화자에 대한 라벨 추가
    speaker_labels.append(1)  # 호출어에 해당하는 클래스 라벨을 1로 추가합니다.

    # 모델 정의 (LSTM 모델)
    model_speaker = tf.keras.models.Sequential([
        tf.keras.layers.LSTM(64, return_sequences=True, input_shape=(X_train_speaker_combined_3d.shape[1], X_train_speaker_combined_3d.shape[2])),
        tf.keras.layers.Dropout(0.5),
        tf.keras.layers.LSTM(64, return_sequences=True),
        tf.keras.layers.Dropout(0.5),
        tf.keras.layers.LSTM(64),
        tf.keras.layers.Dropout(0.5),
        tf.keras.layers.Dense(64, activation='relu'),
        tf.keras.layers.Dense(1, activation='sigmoid')  # 시그모이드 활성화 함수 사용
    ])

    # 모델 컴파일
    model_speaker.compile(
        loss='binary_crossentropy',
        optimizer=tf.keras.optimizers.Adam(learning_rate=0.001),
        metrics=['accuracy']
    )

    # 모델 학습
    model_speaker.fit(X_train_speaker_combined_3d, y_train_speaker, epochs=15, batch_size=16, validation_split=0.2, class_weight=speaker_class_weights)

    # 모델 저장 경로와 이름 설정
    model_directory_speaker = "C:\\Temp\\Server\\" + id
    model_name_speaker = id + "_model"
    model_path_speaker = os.path.join(model_directory_speaker, model_name_speaker + ".h5")

    # 모델 저장
    model_speaker.save(model_path_speaker)


if __name__ == "__main__":
    id = sys.argv[1]
    study_Model(id)