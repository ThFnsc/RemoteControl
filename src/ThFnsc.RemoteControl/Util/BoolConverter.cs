using System.Text.Json.Serialization;

namespace ThFnsc.RemoteControl.Util;

[JsonSerializable(typeof(bool))]
public partial class BoolConverter : JsonSerializerContext;