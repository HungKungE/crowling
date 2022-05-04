using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
namespace crowling
{
    public partial class Form1 : Form
    {
        // 도구 -> NuGet 패키지 관리자 -> 솔루션용 NuGet 패키지 관리 -> 찾아보기에서 HtmlAglityPack 검색 후 프로젝트에 설치
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string url1 = "https://api.finance.naver.com/siseJson.naver?symbol=";
            string url2 = "&requestType=1&startTime=";
            string url3 = "&endTime=";
            string url4 = "&timeframe=week";
            string url_total = url1 + textBox3.Text +url2 + textBox1.Text + url3 + textBox2.Text + url4;
            // textBox3 : 종목코드 , textBox1 : 시작일자, textBox2 : 종료일자
            //url_total 예시 : https://api.finance.naver.com/siseJson.naver?symbol=005930&requestType=1&startTime=20210801&endTime=20210930&timeframe=week
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            String html = wc.DownloadString(url_total); 
            
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(html);
            var div = htmlDoc.Text;
            string remov = "\t";
            string remov2 = "\n [['날짜', '시가', '고가', '저가', '종가', '거래량', '외국인소진율'],\n\n\n\n\n";
            string remov3 = "\n\n\n\n]";
            div = div.Replace(remov, string.Empty);
            div = div.Replace(remov2, string.Empty);
            div = div.Replace(remov3, string.Empty);

            string name = "Stock.txt";

            if (!File.Exists(name)) {
                File.Create(name);
            }

            StreamWriter writer = File.CreateText(name);
            writer.WriteLine(div);
            writer.Close();
            // 시작, 종료, 종목코드를 입력후 버튼 클릭 시 프로젝트의 bin - debug 에 stock.txt가 생기고 정보가 저장됨
            // 처음 실행 시 에러 생길 수 있음, 한번더 실행하면 정상 실행됨
        }
    }
}

//htmlAglityPack
/*
 *             String url = "https://finance.naver.com/item/sise_day.naver?code=005930";
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(url);
            var div = htmlDoc.DocumentNode.SelectNodes("//table[@class='type2']/tr/td/span");
            // dl[@class='blind']/dd -> 당일의 정보
            // span[@class='tah p11'] -> 종가
            var count = 0;
            char[] splitchar = { '\t','\n'};
            foreach (var i in div)
            {
                count++;
                if ((count - 3) % 7 == 0)
                {
                    listBox1.Items.Add(i.InnerText.Split(splitchar)[5]);
                }
                else
                {
                    listBox1.Items.Add(i.InnerText);
                    if (count % 7 == 0)
                    { 
                        listBox1.Items.Add("------------");
                    }
                }
            }
 * 
String url = "https://www.kw.ac.kr/ko/life/notice.jsp?srCategoryId=0";
HtmlWeb web = new HtmlWeb();
var htmlDoc = web.Load(url);

var div = htmlDoc.DocumentNode.SelectNodes("//div[@class='board-text']//a[@href]");
foreach (var i in div) {
    listBox1.Items.Add(i.InnerText.Split('\n')[3].Substring(36));
}

*/
/*
 * 1. 날짜
 * 2. 종가
 * 3. 전일대비 ( +인지 -인지는 계산 구현 )
 * 4. 시가
 * 5. 고가
 * 6. 저가
 * 7. 거래량
 */