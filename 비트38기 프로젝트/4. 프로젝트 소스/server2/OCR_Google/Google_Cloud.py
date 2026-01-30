#파일명 Control1.py
#Google cloud api 사용
from google.oauth2 import service_account
from google.cloud.vision_v1 import types
from google.cloud import vision_v1
from Levenshtein import distance
from dotenv import load_dotenv
import sys
import io
import os

#한글, 영어로 encoding
sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')



#좌표정보, 일치율을 담고있을 calss 정의
class Coordinate:
    def __init__(self, x, y, accuracy, rx, ry):
        #가운데 좌표
        self.x = x
        self.y = y
        #정확도 저장
        self.accuracy = accuracy
        #우측 하단 좌표
        self.rx = rx
        self.ry = ry


def Google_Find_Text(image_path, target_text):
    #google cloud api 진입 함수
    dotenv_path = os.path.join(os.path.dirname(__file__), 'info.env')
    load_dotenv(dotenv_path)

    credentials = service_account.Credentials.from_service_account_file(os.getenv("GOOGLE_API_KEY_PATH"), scopes=[os.getenv("GOOGLE_API_URL")])
    client = vision_v1.ImageAnnotatorClient(credentials=credentials)

    #image_path 에 있는 파일 열기
    with open(image_path, 'rb') as image_file:
        content = image_file.read()

    #image 정보 삽입 및 api로 전송
    image = vision_v1.Image(content=content)
    language_hints = ['ko', 'en']
    image_context = types.ImageContext(language_hints=language_hints)
    response = client.text_detection(image=image, image_context=image_context)

    #받아온 정보에서 text만 추출
    texts = response.text_annotations

    #Coordinate 정보를 담을 배열 선언, 목표 텍스트 split
    Coordinate_list1 = []
    split_text = target_text.split(' ')

    #추출한 texts돌면서 일치정보 있는지 확인하고 확인시 유사도 기입
    for item in texts:
        target_text = split_text[0]
        while target_text != "":
                #lower.()를 이용해 찾은 단어와 찾을 단어를 소문자로 통일
                if item.description.lower() == target_text.lower():
                    #levenshtein 을 사용해 유사율 판단
                    difference = distance(item.description.lower(), split_text[0].lower())

                    #좌표 추출 & 4꼭짓점의 평균값 추출
                    bounding_poly = item.bounding_poly.vertices
                    x_values = sum(vertex.x for vertex in bounding_poly)/4
                    y_values = sum(vertex.y for vertex in bounding_poly)/4

                    x_right = max(vertex.x for vertex in bounding_poly)
                    y_left = max(vertex.y for vertex in bounding_poly)
                    
                    #위에서 만든 class에 정보 기입
                    coordinate_info = Coordinate(x_values, y_values, difference, x_right, y_left)
                    Coordinate_list1.append(coordinate_info)

                #OCR 단어 단위가 무작위임으로 한 글자씩 줄여서 일치하는 텍스트 탐색
                target_text = target_text[:-1]
            
    
    #검색결과 없으면 에러 띄우기----------------------------------------------------
    if not Coordinate_list1:
            #raise 쓰면 return 처럼 함수 탈출함
            #raise Exception('일치하는 텍스트가 없습니다')
            return


    #기입된 정보 중에서 accuracy(일치율)에 대한 정보만 배열로 추출
    accuracy_list1 = [data.accuracy for data in Coordinate_list1]
    #추출된 배열에서 가장 작은 숫자 도출
    min_value1 = min(accuracy_list1)
    #배열 중에서 가장 작은 수의 개수 도출
    count_min1 = accuracy_list1.count(min_value1)


    
    Coordinate_list2 = []
    if(not split_text == "선택"):
        if len(split_text) >=2:
            #두번째 스플릿 된 단어 돌아요~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            #추출한 texts돌면서 일치정보 있는지 확인하고 확인시 유사도 기입
            for item in texts:
                target_text = split_text[1]
                while target_text != "":
                    #lower.()를 이용해 찾은 단어와 찾을 단어를 소문자로 통일
                    if item.description.lower() == target_text.lower():
                        #levenshtein 을 사용해 유사율 판단
                        difference = distance(item.description.lower(), split_text[1].lower())

                        #좌표 추출 & 4꼭짓점의 평균값 추출
                        bounding_poly = item.bounding_poly.vertices
                        x_values = sum(vertex.x for vertex in bounding_poly)/4
                        y_values = sum(vertex.y for vertex in bounding_poly)/4

                        x_right = max(vertex.x for vertex in bounding_poly)
                        y_left = max(vertex.y for vertex in bounding_poly)

                        #위에서 만든 class에 정보 기입
                        coordinate_info = Coordinate(x_values, y_values, difference, x_right, y_left)
                        Coordinate_list2.append(coordinate_info)

                    #OCR 단어 단위가 무작위임으로 한 글자씩 줄여서 일치하는 텍스트 탐색
                    target_text = target_text[:-1]

    #두번째 단어가 있는지 없는지 확인(안에 값이 비어있으면 false 나옴.)
    if(Coordinate_list2):
        #기입된 정보 중에서 accuracy(일치율)에 대한 정보만 배열로 추출
        accuracy_list2 = [data.accuracy for data in Coordinate_list2]
        #추출된 배열에서 가장 정확도가 높은 것
        min_value2 = min(accuracy_list2)
    



    result = []
    for first_word in Coordinate_list1:
        #개수가 1개라면 일치율 가장 높은 단어가 1개 좌표값 반환
        if(count_min1 == 1 and first_word.accuracy == min_value1):
            print(str(first_word.x) + "," + str(first_word.y))
            return True
        #개수가 2개 이상이면서 Coordinate_list2가 존재한다면 추가 비교를 위해 배열에 저장
        if(count_min1 >= 2 and first_word.accuracy == min_value1 and Coordinate_list2):
            for second_word in Coordinate_list2:
                    if(second_word.accuracy == min_value2):
                        #1번째 단어와 2번째 단어의 거리차이를 더해 가장 작은 수 => 같은 줄에있거나 거리가 가깝다 => 문장이다.
                        difference = abs(first_word.x - second_word.x) + abs(first_word.y - second_word.y)*2
                        xcor = (first_word.x+second_word.x)/2
                        ycor = (first_word.y+second_word.y)/2
                        coordinate_info = Coordinate(xcor, ycor, difference, second_word.rx, second_word.ry)
                        result.append(coordinate_info)
        #개수가 2개 이상이라면 일치율이 가장 높은 단어들을 이미지 스크린샷으로 뽑아서 저장
        if(count_min1 >= 2 and first_word.accuracy == min_value1 and not Coordinate_list2):
            print(f"|{first_word.x}, {first_word.y}, {first_word.rx}, {first_word.ry}")


    #두번째 단어가 있을때 + 첫번째 단어만으로 정확한 값을 찾지 못했을때
    if(Coordinate_list2):
        #기입된 정보 중에서 accuracy(일치율)에 대한 정보만 배열로 추출
        accuracy_last = [data.accuracy for data in result]
        #추출된 배열에서 가장 작은 숫자 도출
        accuracy_minvalue = min(accuracy_last)
        #배열 중에서 가장 작은 수의 개수 도출하는 과정
        #완전히 일치하는 픽셀크기의 텍스트여도 2~5정도의 편차가 있어 10 미만은 같은 단어라고 가정, 가장 작은 수에 포함.
        accuracy_mincount = 0
        for wordpoint in result:
            if(abs(wordpoint.accuracy - accuracy_minvalue) <10):
                accuracy_mincount += 1
        
        for wordpoint in result:
            #거리가 가장 작은게 1개 => 우리가 찾던 값 => 좌표값 반환
            if(accuracy_mincount == 1 and wordpoint.accuracy == accuracy_minvalue):
                print(str(wordpoint.x) + "," + str(wordpoint.y))
                return True
            #거리가 같은게 2개 이상=> 똑같은 단어가 있다=> 부분 스크린샷 찍어서 전송
            if(accuracy_mincount >=2 and abs(wordpoint.accuracy - accuracy_minvalue) <10):
                print(f"|{wordpoint.x}, {wordpoint.y}, {wordpoint.rx}, {wordpoint.ry}")
    return True