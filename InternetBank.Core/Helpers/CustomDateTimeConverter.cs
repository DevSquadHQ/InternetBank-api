using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InternetBank.Core.Helpers
{
	public class CustomDateTimeConverter : JsonConverter<DateTime>
	{
		public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			string dateString = reader.GetString();
			DateTime parsedDate;

			// استفاده از TryParse برای قبول فرمت‌های مختلف تاریخ
			if (DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
			{
				return parsedDate; // تاریخ به درستی پارس شد
			}

			// در صورت عدم موفقیت، یک استثنا ایجاد کنید
			return DateTime.MinValue;
		}

		public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString("yyyy-MM-dd")); // تبدیل به فرمت دلخواه
		}
	}
}
