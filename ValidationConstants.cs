namespace IdCard.Hanel
{
    internal static class ValidationConstants
    {
        /// <summary>Số di động Việt Nam: 0 hoặc +84, đầu số 3/5/7/8/9, tiếp theo 8 chữ số.</summary>
        public const string PhonePattern = @"^(0|\+84)(3|5|7|8|9)\d{8}$";
        public const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        public const string PhoneRequired = "Vui lòng nhập số điện thoại.";
        public const string PhoneInvalid = "Số điện thoại không đúng định dạng.";
        public const string EmailRequired = "Vui lòng nhập email.";
        public const string EmailInvalid = "Email không đúng định dạng.";
        public const string ConsentRequired = "Vui lòng đồng ý chính sách điều khoản.";
    }
}
