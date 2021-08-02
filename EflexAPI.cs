class IEflexApi
    {
        private string sClientUserName = "XXXXXXX";
        private string sClientPassword = "XXXXX";
        private string key = "XXXXXX";
        //API 地址
        private string Url = "XXXXXXX";
        //OrderSn 标识订单 sssd123456测试用这个
        public string sClientTxID;
        public string sProductID;
        public int dProductPrice;
        //客户充值的账号
        public string sCustomerAccountNumber;
        //客户手机号
        public string sCustomerMobileNumber;
        public string sDealerMobileNumber = "0182432421";
        //交易标记
        public string sRemark;
        //pin充值才会使用
        public string sOtherParameter;
        //timestamp yyyymmddhhnnss "2019050872359590001";
        public string sTS = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        public string sEncKey;
        public string sResponseID;
        public string sResponseStatus;

        //单号，产品代码，价格，账号
        public IEflexApi(string OrderSn, string sProductID, int dProductPrice, string sCustomerAccountNumber)
        {
            this.sClientTxID = OrderSn;
            this.sProductID = sProductID;
            this.dProductPrice = dProductPrice;
            this.sCustomerAccountNumber = sCustomerAccountNumber;

        }

        public IEflexApi(string sClientUserName, string sClientPassword, string key, string OrderSn, string sProductID, int dProductPrice, string sCustomerAccountNumber)
        {
            this.sClientUserName = sClientUserName;
            this.sClientPassword = sClientPassword;
            this.key = key;
            this.sClientTxID = OrderSn;
            this.sProductID = sProductID;
            this.dProductPrice = dProductPrice;
            this.sCustomerAccountNumber = sCustomerAccountNumber;

        }

        //soap请求方法
        /// <summary>
        /// 发送SOAP请求，并返回响应xml
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="soap">SOAP请求信息</param>
        /// <returns>返回响应信息</returns>
        public string GetSOAPReSource(string soap)
        {

            //发起请求
            WebRequest Wreq = WebRequest.Create(Url);
            HttpWebRequest req = (HttpWebRequest)Wreq;
            req.ContentType = "text/xml; charset=utf-8";
            req.Method = "POST";
            req.Timeout = 30 * 1000;
            using (Stream requestStream = req.GetRequestStream())
            {
                byte[] paramBytes = Encoding.UTF8.GetBytes(soap.ToString());
                requestStream.Write(paramBytes, 0, paramBytes.Length);
            }

            //响应
            var webResponse = req.GetResponse();
            using (StreamReader myStreamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
            {
                string result = "";
                return result = myStreamReader.ReadToEnd();
            }
        }


        //生成时间戳
        public void SetsTS()
        {
            sTS = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
        //生成MD5 KEY
        public void SetsEncKey()
        {
            string strsEncKey = sClientUserName + key + sTS;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] rawData = System.Text.Encoding.UTF8.GetBytes(strsEncKey);
            byte[] result = md5.ComputeHash(rawData);
            sEncKey = BitConverter.ToString(result).Replace("-", "").ToLower();

        }

        //余额查询
        public string CheckBalance()
        {
            //构造soap请求信息
            //API 地址
            StringBuilder soap = new StringBuilder();
            soap.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:tem=\"http://tempuri.org/\">");
            soap.Append("<soapenv:Header/>");
            soap.Append("<soapenv:Body>");
            soap.Append("<tem:CheckBalance>");
            soap.Append("<tem:sClientUserName>" + this.sClientUserName + "</tem:sClientUserName>");
            soap.Append("<tem:sClientPassword>" + this.sClientPassword + "</tem:sClientPassword>");
            soap.Append("</tem:CheckBalance>");
            soap.Append("</soapenv:Body>");
            soap.Append("</soapenv:Envelope>");
            //Console.WriteLine(GetSOAPReSource(soap.ToString()));
            return formatter(GetSOAPReSource(soap.ToString()));
        }


        //充值发送soap请求
        public string RequestTopup()
        {

            //构造soap请求信息
            //创建sEncKey
            SetsEncKey();
            //API 地址
            StringBuilder soap = new StringBuilder();
            soap.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:tem=\"http://tempuri.org/\">");
            soap.Append("<soapenv:Header/>");
            soap.Append("<soapenv:Body>");
            soap.Append("<tem:RequestTopup>");
            soap.Append("<tem:sClientUserName>" + this.sClientUserName + "</tem:sClientUserName>");
            soap.Append("<tem:sClientPassword>" + this.sClientPassword + "</tem:sClientPassword>");
            soap.Append("<tem:sClientTxID>" + this.sClientTxID + "</tem:sClientTxID>");
            soap.Append("<tem:sProductID>" + this.sProductID + "</tem:sProductID>");
            soap.Append("<tem:dProductPrice>" + this.dProductPrice + "</tem:dProductPrice>");
            soap.Append("<tem:sCustomerMobileNumber>" + this.sCustomerAccountNumber + "</tem:sCustomerMobileNumber>");
            soap.Append("<tem:sCustomerAccountNumber>" + this.sCustomerAccountNumber + "</tem:sCustomerAccountNumber>");
            soap.Append("<tem:sDealerMobileNumber>" + this.sDealerMobileNumber + "</tem:sDealerMobileNumber>");
            soap.Append("<tem:sRemark>" + "Gentle" + "</tem:sRemark>");
            soap.Append("<tem:sTS>" + sTS + "</tem:sTS>");
            soap.Append("<tem:sEncKey>" + sEncKey + "</tem:sEncKey>");
            soap.Append("</tem:RequestTopup>");
            soap.Append("</soapenv:Body>");
            soap.Append("</soapenv:Envelope>");
            //Console.WriteLine(formatter(GetSOAPReSource(soap.ToString())));
            return formatter(GetSOAPReSource(soap.ToString()));
        }

        //查询订单信息
        public string CheckTransactionStatusRazer()
        {
            //构造soap请求信息
            //API 地址
            StringBuilder soap = new StringBuilder();
            soap.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:tem=\"http://tempuri.org/\">");
            soap.Append("<soapenv:Header/>");
            soap.Append("<soapenv:Body>");
            soap.Append("<tem:CheckTransactionStatusRazer>");
            soap.Append("<tem:sClientUserName>" + this.sClientUserName + "</tem:sClientUserName>");
            soap.Append("<tem:sClientPassword>" + this.sClientPassword + "</tem:sClientPassword>");
            soap.Append("<tem:sClientTxID>" + this.sClientTxID + "</tem:sClientTxID>");
            soap.Append("</tem:CheckTransactionStatusRazer>");
            soap.Append("</soapenv:Body>");
            soap.Append("</soapenv:Envelope>");
            //Console.WriteLine(formatter(GetSOAPReSource(soap.ToString()))["sResponseStatus"]);
            return formatter(GetSOAPReSource(soap.ToString()));
        }

        //解析soap消息
        public string formatter(string soapResponse)
        {
            string res = "-1";
            string a = "1";
            //获取soap每一对应节点的值
            Regex exprValue = new Regex("(?<=(\\<[A-Za-z]+\\>))([A-Za-z0-9_.]+)(?=(</[A-Za-z]+\\>))");
            MatchCollection dataSets = exprValue.Matches(soapResponse);
            //获取节点名称
            Regex exprTitle = new Regex("(?<=\\<)[A-Za-z]+(?=(\\>[A-Za-z0-9_.]+))");
            MatchCollection Titles = exprTitle.Matches(soapResponse);
            //提取字符串
            if (dataSets.Count > 0)
            {
                Hashtable result = new Hashtable();

                for (var i = 0; i < dataSets.Count; i++)
                {
                    string dataStr = dataSets[i].Value;
                    string titleStr = Titles[i].Value;
                    Console.WriteLine(dataStr == a);
                    result.Add(titleStr, dataStr);
                    if (dataStr == a)
                    {
                        res = "1";
                        return res;
                    }
                }



            }

            return res;
        }
    }