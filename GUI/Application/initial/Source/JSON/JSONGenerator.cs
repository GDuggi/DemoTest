using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace NSRiskManager {
    public class JSONGenerator {
        public string serializedString() {
            StringBuilder sb = new StringBuilder();

            using (TextWriter tw = new StringWriter(sb)) {
                using (JsonWriter jw = new JsonTextWriter(tw)) {
                    jw.Formatting = Formatting.Indented;

                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jw,this);
                }
            }
            return sb.ToString();
        }

        public static byte[] toBytes(JSONGenerator anObj) {
            return toBytes(Encoding.UTF8,anObj);
        }

        static byte[] toBytes(Encoding encoding,JSONGenerator anObj) {
            return toBytes(encoding,anObj.serializedString());
        }

        static byte[] toBytes(Encoding encoding,string p) {
            return encoding.GetBytes(p);
        }

        public byte[] toBytes() {
            return toBytes(this);
        }

        public byte[] toBytes(out string jsonValue) {
            jsonValue = this.serializedString();
            return toBytes(Encoding.UTF8,jsonValue);
        }
    
    }
}