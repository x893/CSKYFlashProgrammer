using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Service
{
    internal class TargetObjectJsonConverter : JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanWrite => true;

        public override bool CanConvert(Type objectType) => objectType.Equals(typeof(TargetObject));

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            JObject jobject = serializer.Deserialize<JObject>(reader);
            TargetObject targetObject = JsonConvert.DeserializeObject<TargetObject>(jobject.ToString());
            object obj = null;
            switch (targetObject.Type)
            {
                case ProgramFileType.Elf:
                    obj = JsonConvert.DeserializeObject<ElfObject>(jobject.ToString());
                    break;
                case ProgramFileType.Hex:
                    obj = JsonConvert.DeserializeObject<HexObject>(jobject.ToString());
                    break;
                case ProgramFileType.Bin:
                    obj = JsonConvert.DeserializeObject<BinObject>(jobject.ToString());
                    break;
                case ProgramFileType.Word:
                    obj = JsonConvert.DeserializeObject<WordObject>(jobject.ToString());
                    break;
                case ProgramFileType.Script:
                    obj = JsonConvert.DeserializeObject<ScriptObject>(jobject.ToString());
                    break;
            }
            return obj;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch ((value as TargetObject).Type)
            {
                case ProgramFileType.Elf:
                    serializer.Serialize(writer, value, typeof(ElfObject));
                    break;
                case ProgramFileType.Hex:
                    serializer.Serialize(writer, value, typeof(HexObject));
                    break;
                case ProgramFileType.Bin:
                    serializer.Serialize(writer, value, typeof(BinObject));
                    break;
                case ProgramFileType.Word:
                    serializer.Serialize(writer, value, typeof(WordObject));
                    break;
                case ProgramFileType.Script:
                    serializer.Serialize(writer, value, typeof(ScriptObject));
                    break;
            }
        }
    }
}