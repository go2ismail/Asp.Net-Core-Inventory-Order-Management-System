namespace coderush.Models.AccountViewModels
{
    public class UserRoleViewModel
    {
        public int CounterId { get; set; }
        public string ApplicationUserId { get; set; }
        public string RoleName { get; set; }
        public bool IsHaveAccess { get; set; }
    }
}
