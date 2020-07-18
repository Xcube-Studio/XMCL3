using Newtonsoft.Json.Linq;

namespace XMCL
{
    public class Json
    {
        static string a = System.IO.Directory.GetCurrentDirectory() + "\\XMCL.json";
        public static JToken Read(string Section, string Name)
        {
            if (System.IO.File.Exists(a))
            {
                string b = System.IO.File.ReadAllText(a);
                try
                {
                    return JObject.Parse(b)[Section][Name];
                }
                catch { return null; }
            }
            else
            {
                return null;
            }
        }
        public static void Write(string Section, string Name, JToken jToken )
        {
            if (System.IO.File.Exists(a))
            {
                string b = System.IO.File.ReadAllText(a);
                try
                {
                    JObject jObject = JObject.Parse(b);
                    jObject[Section][Name] = jToken;
                    System.IO.File.WriteAllText(a, jObject.ToString());
                }
                catch { }
            }
            else
            {
                System.Windows.MessageBox.Show("");
            }
        }
        public static JArray ReadUsers()
        {
            string txt = System.IO.File.ReadAllText(a);
            JObject jObject = JObject.Parse(txt);
            JArray jArray = JArray.Parse(jObject["Login"]["Users"].ToString());
            return jArray;
        }
        public static string ReadUser(string uuid, string Name)
        {
            string txt = System.IO.File.ReadAllText(a);
            JObject jObject = JObject.Parse(txt);
            JArray jArray = JArray.Parse(jObject["Login"]["Users"].ToString());
            for (int i = 0; i < jArray.Count; i++)
            {
                JObject jObject1 = JObject.Parse(jArray[i].ToString());
                if (jObject1["uuid"].ToString() == uuid)
                    return jObject1[Name].ToString();
            }
            return null;
        }
        public static void ChangeUser(string uuid, string Name, string Text)
        {
            string txt = System.IO.File.ReadAllText(a);
            JObject jObject = JObject.Parse(txt);
            JArray jArray = JArray.Parse(jObject["Login"]["Users"].ToString());
            JArray jArray1 = new JArray();
            for (int i = 0; i < jArray.Count; i++)
            {
                JToken jToken = jArray[i];
                JObject jObject1 = (JObject)jToken;
                if (JObject.Parse(jToken.ToString())["uuid"].ToString() == uuid)
                    jObject1[Name] = Text;
                jArray1.Add(jObject1);
            }
            jObject["Login"]["Users"] = jArray1;
            System.IO.File.WriteAllText(a, jObject.ToString());
        }
        public static void AddUsers(string Name, string uuid, string Token, string Mode ,string email)
        {
            string txt = System.IO.File.ReadAllText(a);
            JObject jObject = JObject.Parse(txt);
            JArray jArray = JArray.Parse(jObject["Login"]["Users"].ToString());
            bool HasSame = false;
            for (int i = 0; i < jArray.Count; i++)
            {
                JToken jToken = jArray[i];
                if (JObject.Parse(jToken.ToString())["uuid"].ToString() == uuid)
                    HasSame = true;
            }
            if(HasSame)
            {
                ChangeUser(uuid, "userName", Name);
                ChangeUser(uuid, "uuid", uuid);
                ChangeUser(uuid, "accessToken", Token);
                ChangeUser(uuid, "LoginMode", Mode);
                ChangeUser(uuid, "Email", email);
            }
            else
            {
                JObject jObject1 = new JObject();
                jObject1.Add("userName", Name);
                jObject1.Add("uuid", uuid);
                jObject1.Add("accessToken", Token);
                jObject1.Add("LoginMode", Mode);
                jObject1.Add("Email", email);
                jArray.Add(jObject1);
                jObject["Login"]["Users"] = jArray;
                System.IO.File.WriteAllText(a, jObject.ToString());
            }
        }
        public static void ReMoveUser(string uuid)
        {
            string txt = System.IO.File.ReadAllText(a);
            JObject jObject = JObject.Parse(txt);
            JArray jArray = JArray.Parse(jObject["Login"]["Users"].ToString());
            for (int i=0;i<jArray.Count;i++)
            {
                JToken jToken = jArray[i];
                if (JObject.Parse(jToken.ToString())["uuid"].ToString() == uuid)
                    jArray.Remove(jToken);
            }
            jObject["Login"]["Users"] = jArray;
            System.IO.File.WriteAllText(a, jObject.ToString());
        }

        public static JArray ReadPaths()
        {
            string txt = System.IO.File.ReadAllText(a);
            JObject jObject = JObject.Parse(txt);
            JArray jArray = JArray.Parse(jObject["Files"]["GamePaths"].ToString());
            return jArray;
        }
        public static JToken ReadPath(string Name, string SubName)
        {
            string txt = System.IO.File.ReadAllText(a);
            JObject jObject = JObject.Parse(txt);
            JArray jArray = JArray.Parse(jObject["Files"]["GamePaths"].ToString());
            for (int i = 0; i < jArray.Count; i++)
            {
                JObject jObject1 = JObject.Parse(jArray[i].ToString());
                if (jObject1["Name"].ToString() == Name)
                    return jObject1[SubName];
            }
            return null;
        }
        public static void AddPath(string Name, string Path, string Icon, bool RelativePath)
        {
            string txt = System.IO.File.ReadAllText(a);
            JObject jObject = JObject.Parse(txt);
            JArray jArray = JArray.Parse(jObject["Files"]["GamePaths"].ToString());
            JObject jObject1 = new JObject();
            jObject1.Add("Name", Name);
            jObject1.Add("Path", Path);
            jObject1.Add("Icon", Icon);
            jObject1.Add("RelativePath", RelativePath);
            jArray.Add(jObject1);
            jObject["Files"]["GamePaths"] = jArray;
            System.IO.File.WriteAllText(a, jObject.ToString());
        }
        public static void RemovePath(string Name)
        {
            string txt = System.IO.File.ReadAllText(a);
            JObject jObject = JObject.Parse(txt);
            JArray jArray = JArray.Parse(jObject["Files"]["GamePaths"].ToString());
            for (int i = 0; i < jArray.Count; i++)
            {
                JToken jToken = jArray[i];
                if (JObject.Parse(jToken.ToString())["Files"].ToString() == Name)
                    jArray.Remove(jToken);
            }
            jObject["Login"]["GamePaths"] = jArray;
            System.IO.File.WriteAllText(a, jObject.ToString());
        }

    }
}
