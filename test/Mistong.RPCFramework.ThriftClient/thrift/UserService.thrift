namespace csharp Mistong.Services.UserService

struct UserInfo
{
	1:i32 UserID
	2:string UserName
	3:bool Sex
}

service UserService
{
	UserInfo GetUser(1:i32 UserID)

	list<UserInfo> GetAll();

	bool Add(1:UserInfo user);
}