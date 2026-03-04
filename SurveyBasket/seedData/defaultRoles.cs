namespace SurveyBasket.DAL.seedData;
public static class defaultRoles
{
    public partial class Admin
    {

        public const string Name = nameof(Admin);
        public const string RoleId = "31DBF7C3-FB22-4CBB-A1C5-3E84FC009AD1";
        public const string RoleConcurrencyStamp = "42C40519-7422-4D96-843A-540DFE5E4455";
    }

    public partial class Member
    {

        public const string Name = nameof(Member);
        public const string RoleId = "FA41809D-F0CF-48B4-A8B1-C29C42B9A2C4";
        public const string RoleConcurrencyStamp = "B972488C-A2A7-4BB7-83F6-1FF7D84C09D4";


    }
}
