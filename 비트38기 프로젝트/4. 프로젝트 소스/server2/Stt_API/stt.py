#pip install azure-cognitiveservices-speech
import time
import azure.cognitiveservices.speech as speechsdk
import sys
import io
import os
from dotenv import load_dotenv #pip install python-dotenv
#한글, 영어로 encoding
sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')

#stt.env의 환경변수 load
dotenv_path = os.path.join(os.path.dirname(__file__), 'stt.env')
load_dotenv(dotenv_path)

#API 파일
def speech_recognize_continuous_from_file(filename):
    subscription = os.getenv('SUBSCRIPTION')
    region = os.getenv('REGION')
    speech_config = speechsdk.SpeechConfig(subscription = subscription, region = region)
    speech_config.speech_recognition_language='ko-KR'
    audio_config = speechsdk.audio.AudioConfig(filename = filename)

    speech_recognizer = speechsdk.SpeechRecognizer(speech_config=speech_config, audio_config=audio_config)

    done = False

    def stop_cb(evt):
        #이거 결과 뽑는 코드(대충 코드 끝남, 완료됨 ㅇㅇ 이러는거)
        #근데 이것도 파이썬 결과값으로 들어가기때문에 주석처리함
        #print('CLOSING on {}'.format(evt))
        nonlocal done
        done = True

    speech_recognizer.recognized.connect(lambda evt: print(evt.result.text))
    speech_recognizer.session_stopped.connect(stop_cb)
    speech_recognizer.canceled.connect(stop_cb)

    speech_recognizer.start_continuous_recognition()
    while not done:
        time.sleep(.5)


if __name__ == "__main__":
    id = sys.argv[1]
    sevpath = sys.argv[2]
    filename = sevpath + '\\' + id + '\\record\\lastrec.wav'
    try:
        speech_recognize_continuous_from_file(filename)
    except Exception as e:
        print(e)