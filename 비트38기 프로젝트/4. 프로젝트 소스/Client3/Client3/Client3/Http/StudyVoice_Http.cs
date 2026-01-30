//StudyVoice_Http.cs
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client3.Http
{
    internal class StudyVoice_Http
    {
        // HttpClient 인스턴스 생성
        private HttpClient httpClient = new HttpClient();

        #region 회원가입 시 녹음한 5개의 wav파일 서버로 send
        public async Task SendStudyVoice(string id, string filePath)
        {
            try
            {
                // 지정된 디렉토리에서 .wav 파일을 가져오기
                var wavFiles = Directory.EnumerateFiles(filePath, "*.wav").ToList();

                using (var content = new MultipartFormDataContent())
                {
                    // 텍스트 데이터 추가
                    var textContent = new StringContent(id, Encoding.UTF8);
                    content.Add(textContent, "id");

                    // 5개의 wav 파일을 각각 content에 추가
                    for (int i = 0; i < wavFiles.Count(); i++)
                    {
                        var fileStream = new FileStream(wavFiles[i], FileMode.Open);
                        content.Add(new StreamContent(fileStream), "record" + i, Path.GetFileName(wavFiles[i]));
                    }

                    // 서버로 POST 요청
                    HttpResponseMessage response = await httpClient.PostAsync("http://192.168.17.192:3000/SttRouter/studyvoice", content);

                    //response값 확인
                    if (response.IsSuccessStatusCode)
                    {
                        string responseText = await response.Content.ReadAsStringAsync();

                        //제이슨 형태로 파싱
                        var jsonResponse = JsonDocument.Parse(responseText);

                        string resultValue = jsonResponse.RootElement.GetProperty("result").GetString();
                    }
                    else
                    {
                        //실패한 경우 에러 메시지
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}