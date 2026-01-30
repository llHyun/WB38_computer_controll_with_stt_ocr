//WAV_Http.cs
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Windows.Forms;
using System.IO;
using AutoIt;
using Client3.Member;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection.Configuration;

namespace Client3
{
    // HTTP 및 마우스 컨트롤러 클래스 정의
    public class WAV_Http
    {
        // HttpClient 인스턴스 생성
        private HttpClient httpClient = new HttpClient();

        #region WAV 파일 전송 및 OCR&명령어 호출
        // 텍스트와 이미지 데이터를 업로드하고 응답을 처리하는 메서드
        public async Task SendWAV(string id, string bytepic, string filePath)
        {
            //catch문에서도 사용할 수 있도록 전역 선언
            HttpResponseMessage response = null;
            try
            {
                // 멀티파트 폼 데이터 콘텐츠 생성
                using (var fileStream = new FileStream(filePath, FileMode.Open))
                using (var content = new MultipartFormDataContent())
                {
                    // 텍스트 데이터 추가
                    var textContent = new StringContent(id, Encoding.UTF8);
                    content.Add(textContent, "id");

                    // 이미지 데이터 추가
                    byte[] bytes = Convert.FromBase64String(bytepic);
                    var imageContent = new ByteArrayContent(bytes);
                    imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                    content.Add(imageContent, "image", "image.jpg");

                    Control con = Control.Singleton;

                    //wav파일 전송
                    content.Add(new StreamContent(fileStream), "record", Path.GetFileName(filePath));

                    // 서버로 POST 요청
                    // 화자인식이 체크가 돼있는 경우
                    if (con.speakerBool == true)
                    {
                        response = await httpClient.PostAsync("http://192.168.17.192:3000/SttRouter/tell", content);
                    }
                    // 화자인식이 체크가 안돼있는 경우
                    else if (con.speakerBool == false)
                    {
                        response = await httpClient.PostAsync("http://192.168.17.192:3000/SttRouter/tellnomodel", content);
                    }

                    //response값 확인
                    if (response.IsSuccessStatusCode)
                    {
                        string responseText = await response.Content.ReadAsStringAsync();
                        //제이슨 형태로 파싱
                        var jsonResponse = JsonDocument.Parse(responseText);
                        if (jsonResponse.RootElement.TryGetProperty("Stt_Result", out var SttProperty))
                        {
                            //texts 라면 좌표를 여러개 줬을때
                            //서버에서 담아준 내용 파싱
                            string sentence = jsonResponse.RootElement.GetProperty("Stt_Result").GetString();

                            //받은 내용 화면에 띄우기.
                            con.Stt_Result = sentence;
                            if (sentence.Contains("선택"))
                            {
                                var sttTask = Task.Run(() => con.Stt_Form());
                                await con.SendOCR(con.Id, sentence);
                            }
                            else
                            {
                                #region 명령어 전달 전 일치 항목 확인
                                List<Macro> mlist = con.mList;

                                string cmd = null;

                                foreach (var m in mlist)
                                {
                                    if (sentence.Contains(m.Cmd))
                                    {
                                        cmd = m.Cmd;
                                    }
                                }

                                if (cmd == null)
                                {
                                    con.Load();
                                    var sttTask = Task.Run(() => con.Stt_Form());
                                    return;
                                }
                                #endregion
                                else
                                {
                                    string code = await con.SelectCode(con.Id, cmd);
                                    AutoItX.Send(code);
                                    var sttTask = Task.Run(() => con.Stt_Form());
                                }
                            }
                        }
                        else if (jsonResponse.RootElement.TryGetProperty("empty", out var emptyProperty))
                        {
                            //공백 음성 데이터가 들어가 공백이 반환되었을 때
                        }
                        else if (jsonResponse.RootElement.TryGetProperty("nospeak", out var nospeakProperty))
                        {
                            //화자가 아닐때
                            con.Stt_Result = "화자가 아닙니다.";
                            var sttTask = Task.Run(() => con.Stt_Form());
                        }
                        else if (jsonResponse.RootElement.TryGetProperty("nomodel", out var nomodelProperty))
                        {
                            //모델에서 값을 안줬을때
                            con.Stt_Result = "잘 알아듣지 못했습니다..";
                            var sttTask = Task.Run(() => con.Stt_Form());
                        }
                        else if (jsonResponse.RootElement.TryGetProperty("error", out var errorProperty))
                        {
                            string err = jsonResponse.RootElement.GetProperty("error").GetString();
                            MessageBox.Show(err);
                        }
                        else
                        {
                            con.Stt_Result = "서버로 부터 값을 전달받지 못했습니다.";
                            var sttTask = Task.Run(() => con.Stt_Form());
                        }
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
                Control control = Control.Singleton;

                //try문에서 받은 response값에 오류가 들어있다면 그 오류 반환 / 아니라면 ex.Message
                string errorMessage = response != null ? await response.Content.ReadAsStringAsync() : ex.Message;

                //json코드로 변환 가능한지 확인(만약 ex.Message가 들어간다면 오류나기 때문)
                if (control.IsValidJson(errorMessage))
                {
                    using (JsonDocument document = JsonDocument.Parse(errorMessage))
                    {
                        //내부에서 text값을 찾는 과정
                        if (document.RootElement.TryGetProperty("text", out JsonElement textProperty))
                        {
                            string msgerr = textProperty.GetString();
                            MessageBox.Show(msgerr);
                        }
                        else
                        {
                            //text값이 존재하지 않을때
                            MessageBox.Show("서버로 부터 에러에 대한 값을 전달받지 못했습니다.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
            Control cotn = Control.Singleton;
            cotn.Load();
        }
        #endregion
    }
}