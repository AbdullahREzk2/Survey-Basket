global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
global using SurveyBasket.BLL.Contracts;
global using SurveyBasket.BLL.Service;
global using SurveyBasket.BLL.IService;
global using SurveyBasket.DAL.IRepository;
global using Mapster;
global using MapsterMapper;
global using SurveyBasket.DAL.Entities;
global using FluentValidation;
global using SurveyBasket.BLL.Contracts.Authantication;
global using Microsoft.AspNetCore.Identity;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using Microsoft.IdentityModel.Tokens;
global using SurveyBasket.BLL.CurrentUserService;
global using Microsoft.Extensions.Options;
global using System.ComponentModel.DataAnnotations;
global using System.Security.Cryptography;
global using SurveyBasket.BLL.Contracts.Polls;
global using SurveyBasket.BLL.Abstractions;
global using SurveyBasket.BLL.Errors;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.Extensions.Logging;
global using SurveyBasket.BLL.Contracts.Questions;
global using SurveyBasket.BLL.Contracts.Answers;
global using SurveyBasket.BLL.Contracts.Votes;
global using SurveyBasket.BLL.Contracts.Dashboard;



