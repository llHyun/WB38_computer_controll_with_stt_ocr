# 파일명: app.py
from Google_Cloud import Google_Find_Text
import sys
from nltk.tokenize import word_tokenize
import io
sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')

def remove_stopwords(text, stop_words):
    # 주어진 텍스트에서 불용어를 제거하는 함수
    word_tokens = word_tokenize(text)
    result = [word for word in word_tokens if word not in stop_words]
    
    return result

def load_stopwords(file_path):
    # 파일에서 불용어를 읽어와 리스트로 반환하는 함수
    with open(file_path, 'r', encoding='utf-8') as f:
        stopwords = f.read().split(' ')
    
    # 중복된 불용어 제거
    stopwords = list(set(stopwords))
    
    return stopwords


def start_stopwords(text):
    stop_words = load_stopwords('./Stop_words/stopwords_kor.txt')  # 불용어가 저장된 텍스트 파일 경로
    result = remove_stopwords(text, stop_words)

    result_sentence = ' '.join(result)  # 리스트를 공백으로 연결하여 문자열로 변환
    return result_sentence 

#---------------------------------------------------
#이 위에 3가지 함수는 불용어 처리를 위한 함수. stop_words에 있는 txt파일을 가져와서 불용어를 처리한다.



if __name__ == "__main__":
    #파이썬을 실행할때 인자로 줬던 값들을 가져올 수 있는 코드
    text = sys.argv[1]
    imagePath = sys.argv[2]
    #불용어 처리를 사용하여 정제된 문장을 OCR API로 전송
    processed_text = start_stopwords(text)
    if(text.strip()):
        
        try:
            Google_Find_Text(imagePath, processed_text)
        except Exception as e:
            print(e)
    else:
        print("값 안들어옴요")

