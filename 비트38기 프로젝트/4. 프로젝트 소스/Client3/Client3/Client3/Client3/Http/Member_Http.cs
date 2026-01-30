using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client3.Http
{
    internal class Member_Http
    {
        // HttpClient 인스턴스 생성
        private HttpClient httpClient = new HttpClient();
        #region 회원가입, 로그인 + 회원정보 수정 + 회원탈퇴
        //회원가입
        public async Task<bool> Signup(string accinfo)
        {
            try
            {
                // 멀티파트 폼 데이터 콘텐츠 생성
                using (var content = new MultipartFormDataContent())
                {
                    var textContent = new StringContent(accinfo, Encoding.UTF8);
                    content.Add(textContent, "text");

                    // 서버로 POST 요청
                    HttpResponseMessage response = await httpClient.PostAsync("http://localhost:3000/LoginRouter/signup", content);
                    //response값 확인
                    if (response.IsSuccessStatusCode)
                    {
                        string responseText = await response.Content.ReadAsStringAsync();
                        //제이슨 형태로 파싱
                        var jsonResponse = System.Text.Json.JsonDocument.Parse(responseText);

                        string resultValue = jsonResponse.RootElement.GetProperty("result").GetString();

                        // result 값이 true일 경우
                        if (resultValue == "true")
                        {
                            return true;
                        }
                        // result 값이 duplication일 경우
                        else if (resultValue == "duplication")
                        {
                            MessageBox.Show("아이디 중복");
                            return false;
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
        //자동 로그인
        public async Task<(bool, string)> auto(string token)
        {
            try
            {
                // 멀티파트 폼 데이터 콘텐츠 생성
                using (var content = new MultipartFormDataContent())
                {
                    var textContent = new StringContent(token, Encoding.UTF8);
                    content.Add(textContent, "token");
                    // 서버로 POST 요청
                    HttpResponseMessage response = await httpClient.PostAsync("http://localhost:3000/LoginRouter/autologin", content);
                    //response값 확인
                    if (response.IsSuccessStatusCode)
                    {
                        string responseText = await response.Content.ReadAsStringAsync();
                        //제이슨 형태로 파싱
                        var jsonResponse = System.Text.Json.JsonDocument.Parse(responseText);

                        string resultValue = jsonResponse.RootElement.GetProperty("result").GetString();

                        if (resultValue == "true")
                        {
                            string userId = jsonResponse.RootElement.GetProperty("id").GetString();
                            return (true, userId);
                        }
                        else
                        {
                            return (false, null);
                        }
                    }
                    else
                    {
                        //실패한 경우 에러 메시지
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(errorMessage);
                        return (false, null);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return (false, null);
            }
        }
        //로그인 
        public async Task<(bool, string)> Login(string set)
        {
            try
            {
                // 멀티파트 폼 데이터 콘텐츠 생성
                using (var content = new MultipartFormDataContent())
                {
                    var textContent = new StringContent(set, Encoding.UTF8);
                    content.Add(textContent, "text");

                    // 서버로 POST 요청
                    HttpResponseMessage response = await httpClient.PostAsync("http://localhost:3000/LoginRouter/login", content);
                    //response값 확인
                    if (response.IsSuccessStatusCode)
                    {
                        string responseText = await response.Content.ReadAsStringAsync();
                        //제이슨 형태로 파싱
                        var jsonResponse = System.Text.Json.JsonDocument.Parse(responseText);

                        string resultValue = jsonResponse.RootElement.GetProperty("result").GetString();

                        if (resultValue == "true")
                        {
                            string token = jsonResponse.RootElement.GetProperty("token").GetString();
                            File.WriteAllText("token.dat", token);
                            string userId = jsonResponse.RootElement.GetProperty("id").GetString();
                            return (true, userId);
                        }
                        else
                        {
                            return (false, null);
                        }
                    }
                    else
                    {
                        //실패한 경우 에러 메시지
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(errorMessage);
                        return (false, null);
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return (false, null);
            }
        }
        //회원탈퇴
        public async Task<bool> Withdraw(string accinfo)
        {
            try
            {
                // 멀티파트 폼 데이터 콘텐츠 생성
                using (var content = new MultipartFormDataContent())
                {
                    var textContent = new StringContent(accinfo, Encoding.UTF8);
                    content.Add(textContent, "text");

                    // 서버로 POST 요청
                    HttpResponseMessage response = await httpClient.PostAsync("http://localhost:3000/UserMenuRouter/withdraw", content);
                    //response값 확인
                    if (response.IsSuccessStatusCode)
                    {
                        string responseText = await response.Content.ReadAsStringAsync();
                        //제이슨 형태로 파싱
                        var jsonResponse = System.Text.Json.JsonDocument.Parse(responseText);

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
        //회원정보 수정
        public async Task<bool> Update(string accinfo)
        {
            try
            {
                // 멀티파트 폼 데이터 콘텐츠 생성
                using (var content = new MultipartFormDataContent())
                {
                    var textContent = new StringContent(accinfo, Encoding.UTF8);
                    content.Add(textContent, "text");

                    // 서버로 POST 요청
                    HttpResponseMessage response = await httpClient.PostAsync("http://localhost:3000/UserMenuRouter/user_update", content);
                    //response값 확인
                    if (response.IsSuccessStatusCode)
                    {
                        string responseText = await response.Content.ReadAsStringAsync();
                        //제이슨 형태로 파싱
                        var jsonResponse = System.Text.Json.JsonDocument.Parse(responseText);

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
