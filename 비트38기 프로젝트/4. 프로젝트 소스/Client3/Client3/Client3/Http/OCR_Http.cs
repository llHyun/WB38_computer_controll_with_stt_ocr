//OCR_Http.cs
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
    internal class OCR_Http
    {
        private HttpClient httpClient = new HttpClient();

        #region OCR 요청
        public async Task SendOCR(string id, string result)
        {
            //catch문에서도 사용할 수 있도록 전역 선언
            HttpResponseMessage response = null;
            try
            {
                // 멀티파트 폼 데이터 콘텐츠 생성
                using (var content = new MultipartFormDataContent())
                {
                    // 텍스트 데이터 추가
                    var idContent = new StringContent(id, Encoding.UTF8);
                    content.Add(idContent, "id");

                    var textContent = new StringContent(result, Encoding.UTF8);
                    content.Add(textContent, "result");

                    // 서버로 POST 요청
                    response = await httpClient.PostAsync("http://192.168.17.192:3000/OcrRouter/upload", content);

                    Control con = Control.Singleton;

                    //response값 확인
                    if (response.IsSuccessStatusCode)
                    {
                        string responseText = await response.Content.ReadAsStringAsync();
                        //제이슨 형태로 파싱
                        var jsonResponse = System.Text.Json.JsonDocument.Parse(responseText);
                        if (jsonResponse.RootElement.TryGetProperty("texts", out var textsProperty))
                        {
                            //texts에서 좌표를 여러개 줬을때
                            //서버에서 담아준 내용 파싱
                            string sentence = jsonResponse.RootElement.GetProperty("texts").GetString();
                            string[] texts = sentence.Split('|');
                            con.coordinates.Clear();

                            for (int i = 1; i < texts.Length; i++)
                            {
                                string[] text = texts[i].Split('\\');
                                string[] cor = text[0].Split(',');
                                string lastttttt = cor[0].Split('.')[0] + "," + cor[1].Split('.')[0];
                                con.coordinates.Add(new Coordinate(int.Parse(cor[0].Split('.')[0]), int.Parse(cor[1].Split('.')[0]),
                                    int.Parse(cor[2].Split('.')[0]), int.Parse(cor[3].Split('.')[0])));
                            }

                            //라벨이 떴다는 걸 알려주는 코드
                            con.IsLabel = true;

                            //라벨 띄우는 코드
                            var labelStack = Task.Run(() => con.Label_Form(con.coordinates, con));
                        }
                        else if (jsonResponse.RootElement.TryGetProperty("text", out var textProperty))
                        {
                            //text라면 좌표값이 하나만 온 것
                            //위와 같이 받아온 데이터 split
                            string textData = jsonResponse.RootElement.GetProperty("text").GetString();
                            string[] text = textData.Split('\\');
                            string[] cor = text[0].Split(',');

                            //좌표로 이동후 이벤트 부여
                            con.mouse_Move(int.Parse(cor[0].Split('.')[0]), int.Parse(cor[1].Split('.')[0]));
                            con.mouse_DClick();
                        }
                        else if (jsonResponse.RootElement.TryGetProperty("emptyocr", out var emptyocrProperty))
                        {
                            con.Stt_Result = "일치하는 단어가 존재하지 않습니다.";
                            var sttTask = Task.Run(() => con.Stt_Form());
                        }
                        else if (jsonResponse.RootElement.TryGetProperty("error", out var errorProperty))
                        {

                        }
                        else
                        {
                            MessageBox.Show("서버로부터 값을 받지 못했습니다.");
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
                            MessageBox.Show("서버로 부터 값을 전달받지 못했습니다.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        #endregion

        #region OCR 중복시 라벨 음성처리
        // 텍스트와 이미지 데이터를 업로드하고 응답을 처리하는 메서드
        public async Task SendTTN(string id, string filePath)
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

                    //wav파일 전송
                    content.Add(new StreamContent(fileStream), "record", Path.GetFileName(filePath));

                    // 서버로 POST 요청
                    response = await httpClient.PostAsync("http://192.168.17.192:3000/SttRouter/ttn", content);

                    Control con = Control.Singleton;
                    //response값 확인
                    
                    if (response.IsSuccessStatusCode)
                    {
                        string responseText = await response.Content.ReadAsStringAsync();

                        if (responseText != null)
                        {
                            //STT의 결과값 중 한글로 숫자를 말한 게 있다면 해당 글자를 숫자로 변환하여 리턴
                            int findnum = con.ChangeToNum(responseText);
                            int secnum = con.FindNum(responseText);

                            for (int i = 0; i < con.coordinates.Count(); i++)
                            {
                                if (findnum == i + 1 || secnum == i + 1)
                                {
                                    con.LabelForm.Invoke(new Action(() =>
                                    {
                                        con.LabelForm.Hide();
                                        con.mouse_Move(con.coordinates[i].MiddleX, con.coordinates[i].MiddleY);
                                        con.mouse_DClick();
                                        con.LabelForm.Close();
                                        con.IsLabel = false;
                                    }));
                                }
                            }
                        }
                        
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
                            MessageBox.Show("서버로 부터 값을 전달받지 못했습니다, 재시작 요망.");
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