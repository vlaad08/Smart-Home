﻿using DBComm.Shared;

namespace WebAPI.Service;

public interface IAuthService
{
    Task<Admin> GetAdmin(string login, string password);
    Task RegisterMember(Member member);
    Task<Member> GetMember(String login, string password);

}