//Macro_Http.cs
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client3.Http
{
    internal class Macro_Http
    {
        // HttpClient 인스턴스 생성
        private HttpClient httpClient = new HttpClient();

        #region select code to id
        //id와 cmd를 통해서 명령어(실행코드)를 받아오는 과정
        public async Task<string> SelectCodeToID(string id, string cmd)
        {
            try
            {
                // 멀티파트 폼 데이터 콘텐츠 생성
                using (var content = new MultipartFormDataContent())
                {
                    string value = id + "#" + cmd;

                    var textContent = new StringContent(value, Encoding.UTF8);
                    content.Add(textContent, "text");

                    // 서버로 POST 요청
                    HttpResponseMessage response = await httpClient.PostAsync("http://192.168.17.192:3000/MacroRouter/selectcode", content);
                    //response값 확인
                    if (response.IsSuccessStatusCode)
                    {
                        string responseText = await response.Content.ReadAsStringAsync();

                        //제이슨 형태로 파싱
                        var jsonResponse = JsonDocument.Parse(responseText);

                        string resultValue = jsonResponse.RootElement.GetProperty("result").GetString();

                        return resultValue;
                    }
                    else
                    {
                        //실패한 경우 에러 메시지
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(errorMessage);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        #endregion

        #region selectall to id
        // id로 리스트 받아오기
        public async Task<string> SelectAllToID(string id)
        {
            try
            {
                // 멀티파트 폼 데이터 콘텐츠 생성
                using (var content = new MultipartFormDataContent())
                {
                    var textContent = new StringContent(id, Encoding.UTF8);
                    content.Add(textContent, "text");

                    // 서버로 POST 요청
                    HttpResponseMessage response = await httpClient.PostAsync("http://192.168.17.192:3000/MacroRouter/selectall", content);
                    //response값 확인
                    if (response.IsSuccessStatusCode)
                    {
                        string responseText = await response.Content.ReadAsStringAsync();
                        //제이슨 형태로 파싱
                        var jsonResponse = JsonDocument.Parse(responseText);

                        string resultValue = jsonResponse.RootElement.GetProperty("result").GetString();

                        return resultValue;
                    }
                    else
                    {
                        //실패한 경우 에러 메시지
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(errorMessage);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        #endregion

        #region insert
        // 매크로 저장
        public async Task<bool> InsertMacro(string id, string cmd, string key)
        {
            try
            {
                // 멀티파트 폼 데이터 콘텐츠 생성
                using (var content = new MultipartFormDataContent())
                {
                    string value = id + "#" + cmd + "#" + key;

                    var textContent = new StringContent(value, Encoding.UTF8);
                    content.Add(textContent, "text");

                    // 서버로 POST 요청
                    HttpResponseMessage response = await httpClient.PostAsync("http://192.168.17.192:3000/MacroRouter/insertmacro", content);

                    //response값 확인
                    if (response.IsSuccessStatusCode)
                    {
                        string responseText = await response.Content.ReadAsStringAsync();
                        //제이슨 형태로 파싱
                        var jsonResponse = JsonDocument.Parse(responseText);

                        string resultValue = jsonResponse.RootElement.GetProperty("result").GetString();

                        // result 값이 true일 경우
                        if (resultValue == "true")
                        {
                            return true;
                        }
                        // 그외의 result 값
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        //실패한 경우 에러 메시지
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(errorMessage);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        #endregion

        #region delete
        // 매크로 삭제
        public async Task<bool> DeleteMacro(string id, string key)
        {
            try
            {
                // 멀티파트 폼 데이터 콘텐츠 생성
                using (var content = new MultipartFormDataContent())
                {
                    string value = id + "#" + key;

                    var textContent = new StringContent(value, Encoding.UTF8);
                    content.Add(textContent, "text");

                    // 서버로 POST 요청
                    HttpResponseMessage response = await httpClient.PostAsync("http://192.168.17.192:3000/MacroRouter/deletemacro", content);
                    //response값 확인
                    if (response.IsSuccessStatusCode)
                    {
                        string responseText = await response.Content.ReadAsStringAsync();
                        //제이슨 형태로 파싱
                        var jsonResponse = JsonDocument.Parse(responseText);

                        string resultValue = jsonResponse.RootElement.GetProperty("result").GetString();

                        // result 값이 true일 경우
                        if (resultValue == "true")
                        {
                            return true;
                        }
                        // 그외의 result 값
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        //실패한 경우 에러 메시지
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(errorMessage);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        #endregion

        #region update
        // 매크로 수정
        public async Task<bool> UpdateMacro(string id, string ocmd, string cmd, string key)
        {
            try
            {
                // 멀티파트 폼 데이터 콘텐츠 생성
                using (var content = new MultipartFormDataContent())
                {
                    string value = id + "#" + ocmd + "#" + cmd + "#" + key;

                    var textContent = new StringContent(value, Encoding.UTF8);
                    content.Add(textContent, "text");

                    // 서버로 POST 요청
                    HttpResponseMessage response = await httpClient.PostAsync("http://192.168.17.192:3000/MacroRouter/updatemacro", content);

                    //response값 확인
                    if (response.IsSuccessStatusCode)
                    {
                        string responseText = await response.Content.ReadAsStringAsync();
                        //제이슨 형태로 파싱
                        var jsonResponse = JsonDocument.Parse(responseText);

                        string resultValue = jsonResponse.RootElement.GetProperty("result").GetString();

                        // result 값이 true일 경우
                        if (resultValue == "true")
                        {
                            return true;
                        }
                        // 그외의 result 값
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        //실패한 경우 에러 메시지
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(errorMessage);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        #endregion
    }
}