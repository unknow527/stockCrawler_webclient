using System.Net;
using System.Text;
using System;
using HtmlAgilityPack;
using System.Net;

//安裝套件HtmlAgilityPack.NetCore，取得網頁內容與提取資料
//安裝套件RegularExpressions，取得或移除字串內容
class Program
{
    // 抓取goodinfo 股票名稱

    static void Main(string[] args)
    {
        string stockId = "2330";
        //Get_Company_Info(stockId);
        Get_ChartFlow_Info(stockId);
        //Get_Yahoo_Info(stockId);

        Console.WriteLine("Press Any Key...");
        Console.ReadKey(true); //Pause
    }

    // 抓取Yahooe股市資料
    static void Get_Yahoo_Info(string id)
    {
        string url = "https://tw.stock.yahoo.com/quote/";
        //string id = "2330";


        WebClient client = new WebClient();
        client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36)");

        //將網頁來源資料暫存到記憶體內
        MemoryStream ms = new MemoryStream(client.DownloadData(url + id));

        HtmlDocument doc = new HtmlDocument();
        doc.Load(ms, Encoding.UTF8); // 使用 UTF8 編碼讀入 HTML

        // 檢查資料，將stream轉字串
        string str = System.Text.Encoding.Default.GetString(ms.ToArray());
        Console.WriteLine(str);


        //目標網站的XPath、去除瀏覽器文本會自動產生tbody
        string Xpath = "/html/body/div[1]/div/div/div/div/div[5]/div[1]/div[1]/div/div[1]/div/div[1]/h1";
        string nXpath = Xpath.Replace("/tbody", "");
        //Thread.Sleep(500);

        // 取得網站的公司名稱
        string txt = doc.DocumentNode.SelectSingleNode(nXpath).InnerText.ToString();


        Console.WriteLine(txt);

        string CompanyName = txt;
        Console.WriteLine(CompanyName);
    }

    // 抓取goodinfo 本淨比河流圖
    static void Get_ChartFlow_Info(string id)
    {
        string url = "https://goodinfo.tw/tw/ShowK_ChartFlow.asp?RPT_CAT=PBR&STOCK_ID="+id+"&CHT_CAT=MONTH";
        //string id = "2330";
        Console.WriteLine(url);

        WebClient client = new WebClient();
        client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36)");

        //將網頁來源資料暫存到記憶體內
        MemoryStream ms = new MemoryStream(client.DownloadData(url + id));

        HtmlDocument doc = new HtmlDocument();
        doc.Load(ms, Encoding.UTF8); // 使用 UTF8 編碼讀入 HTML

        // 檢查資料，將stream轉字串
        string str = System.Text.Encoding.Default.GetString(ms.ToArray());
        //Console.WriteLine(str);
        //Console.WriteLine("all");

        // 目標網站的XPath、去除瀏覽器文本會自動產生tbody
        string Xpath = "/html/body/table[2]/tbody/tr/td[3]/div/div/div/table/tbody/tr[3]";
        string nXpath = Xpath.Replace("/tbody", "");
        Thread.Sleep(500);


        // 取得網站要的資料

        // 清理前資料
        string data = doc.DocumentNode.SelectSingleNode(nXpath).InnerHtml.ToString();
        Console.WriteLine(data);

        // 清理後資料

        // 1-日期/2-收盤價/3-漲跌價/4-漲跌幅/5-河流圖BPS(元)/6-目前PBR(倍)/
        // 7-本淨比L1/8-本淨比L2/9-本淨比L3/10-本淨比L4/11-本淨比L5/12-本淨比L6
        //
        // # 沒辦法用以下方式，分次階段取資料?
        // doc = doc.DocumentNode.SelectSingleNode(nXpath)
        // doc = doc.DocumentNode.SelectSingleNode("/td[1]")
        string date = doc.DocumentNode.SelectSingleNode(nXpath+ "/td[1]").InnerText.ToString();
        string price = doc.DocumentNode.SelectSingleNode(nXpath + "/td[2]").InnerText.ToString();
        string updown = doc.DocumentNode.SelectSingleNode(nXpath + "/td[3]").InnerText.ToString();
        string updown_percent = doc.DocumentNode.SelectSingleNode(nXpath + "/td[4]").InnerText.ToString();
        string bps = doc.DocumentNode.SelectSingleNode(nXpath + "/td[5]").InnerText.ToString();
        string bps_percent = doc.DocumentNode.SelectSingleNode(nXpath + "/td[6]").InnerText.ToString();
        string pb_l1= doc.DocumentNode.SelectSingleNode(nXpath + "/td[7]").InnerText.ToString();
        string pb_l2 = doc.DocumentNode.SelectSingleNode(nXpath + "/td[8]").InnerText.ToString();
        string pb_l3 = doc.DocumentNode.SelectSingleNode(nXpath + "/td[9]").InnerText.ToString();
        string pb_l4 = doc.DocumentNode.SelectSingleNode(nXpath + "/td[10]").InnerText.ToString();
        string pb_l5 = doc.DocumentNode.SelectSingleNode(nXpath + "/td[11]").InnerText.ToString();
        string pb_l6 = doc.DocumentNode.SelectSingleNode(nXpath + "/td[12]").InnerText.ToString();

        List<string> list = new List<string>() { date, price, updown, updown_percent, bps, bps_percent, pb_l1, pb_l2, pb_l3, pb_l4, pb_l5, pb_l6 };
        string val = list.Aggregate((x, y) => x+","+y);
        Console.WriteLine(val);
   
    }

}

