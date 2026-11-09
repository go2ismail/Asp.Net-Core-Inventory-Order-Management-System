namespace Domain.Common;

public static class Constants
{
    public static class LengthConsts
    {
        public const int S = 50;
        public const int M = 255;
        public const int L = 2000;
        public const int XL = 4000;
    }
    public static class NameConsts
    {
        public const int MinLength = 2;
        public const int MaxLength = LengthConsts.M;
    }
    public static class CodeConsts
    {
        public const int MinLength = 1;
        public const int MaxLength = LengthConsts.S;
    }
    public static class DescriptionConsts
    {
        public const int MaxLength = LengthConsts.XL;
    }
    public static class EmailConsts
    {
        public const int MinLength = 6;
        public const int MaxLength = LengthConsts.M;
    }
    public static class IdConsts
    {
        public const int MaxLength = LengthConsts.S;
    }
    public static class PasswordConsts
    {
        public const int MinLength = 6;
        public const int MaxLength = LengthConsts.M;
    }
    public static class PathConsts
    {
        public const int MaxLength = LengthConsts.M;
    }
    public static class TokenConsts
    {
        public const int MaxLength = LengthConsts.XL;
        public const double ExpiryInDays = 30;
    }
    public static class UserIdConsts
    {
        public const int MaxLength = 450;
    }
}

