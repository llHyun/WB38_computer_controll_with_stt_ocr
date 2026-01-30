//ChangeText.cs
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Client3
{
    internal class ChangeText
    {
        public int ChangeToNum(string input)
        {
            Dictionary<char, int> numberMapping = new Dictionary<char, int>()
            {
                {'일', 1},
                {'이', 2},
                {'삼', 3},
                {'사', 4},
                {'오', 5},
                {'육', 6},
                {'칠', 7},
                {'팔', 8},
                {'구', 9},
                {'십', 10}
                // 추가적인 문자와 숫자의 매핑이 있다면 여기에 계속 추가 가능
            };

            int currentNumber = 0;
            for (int i = 0; i < input.Length; i++)
            {
                char currentCharacter = input[i];
                if (currentCharacter == '십')
                {
                    if (i == 0)
                    {
                        //맨 앞자리가 십 일때
                        currentNumber = 10;
                    }
                    else
                    {
                        char preNumber = input[i - 1];
                        if (numberMapping.ContainsKey(preNumber))
                        {
                            //십 앞에 숫자가 있을때
                            currentNumber = numberMapping[preNumber] * 10;
                        }
                        else
                        {
                            //십 앞에 숫자가 없을때
                            currentNumber = 10;
                        }
                    }
                    //다음 글자 확인
                    int nextNumber = 0;
                    if (i < input.Length - 1)
                    {
                        char nextCharacter = input[i + 1];
                        if (numberMapping.ContainsKey(nextCharacter))
                        {
                            // 다음 글자도 숫자와 관련된 글자인 경우
                            nextNumber = numberMapping[nextCharacter];
                        }
                    }
                    return currentNumber + nextNumber;

                }
            }

            for (int i = 0; i < input.Length; i++)
            {
                char currentCharacter = input[i];

                if (numberMapping.ContainsKey(currentCharacter))
                {
                    //현재 문자가 숫자일 경우
                    currentNumber = numberMapping[currentCharacter];
                }
            }
            return currentNumber;

        }

        public int FindNum(string input)
        {
            string pattern = @"\d+";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(input);

            // 추출된 숫자를 하나의 문자열로 합치기
            if (match.Success)
            {
                // 추출된 숫자를 int로 변환
                try
                {
                    return int.Parse(match.Value);
                }
                catch (FormatException)
                {
                    // 숫자로 변환할 수 없는 경우 기본값 0 반환
                    return 0;
                }
            }

            // 추출된 숫자가 없으면 기본값 0 반환
            return 0;
        }
    }
}