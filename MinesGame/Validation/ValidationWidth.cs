using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MinesGame.Validation
{
    public class ValidationWidth : ValidationRule

    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int num = 0;
            if (int.TryParse(value.ToString(), out num))
            {
                if (num >= 9 && num <= 30)
                    return new ValidationResult(true, null);
                return new ValidationResult(false, "请输入9~30之间的数字！");
            }
            else
                return new ValidationResult(false, "数字格式错误！");
            //throw new NotImplementedException();
        }
    }
}
