﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service.DynamicsCrm.WebHost.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using AutoMapper;
using Microsoft.Xrm.Sdk;
using Microsoft.WindowsAzure.Mobile.Service.Tables;
using Microsoft.WindowsAzure.Mobile.Service.Security.Providers;

namespace Microsoft.WindowsAzure.Mobile.Service.DynamicsCrm.WebHost
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            ConfigOptions options = new ConfigOptions();
            options.LoginProviders.Remove(typeof(AzureActiveDirectoryLoginProvider));
            options.LoginProviders.Add(typeof(AzureActiveDirectoryExtendedLoginProvider));

            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));

            // enforce user authentication even when debugging locally
            config.SetIsHosted(true);

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            // config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            var map = Mapper.CreateMap<Account, AccountDto>();
            AutoMapperEntityMapper.InitializeDynamicsCrmCommonMaps();

            map.ForMember(dto => dto.City, opt => opt.MapFrom(crm => crm.Address1_City))
                .ForMember(dto => dto.CreatedAt, opt => opt.MapFrom(crm => (DateTimeOffset?)crm.CreatedOn))
                .ForMember(dto => dto.UpdatedAt, opt => opt.MapFrom(crm => (DateTimeOffset?)crm.ModifiedOn))
                .ForMember(dto => dto.ParentAccountId, opt => opt.MapFrom(crm => crm.ParentAccountId))
                .ForMember(dto => dto.ParentAccountType, opt => opt.MapFrom(crm => crm.ParentAccountId))
                .ForMember(dto => dto.IndustryCode, opt => opt.MapFrom(crm => crm.IndustryCode));
            
            var reverseMap = map.ReverseMap();
            reverseMap.ForMember(crm => crm.Address1_City, opt => opt.MapFrom(dto => dto.City))
                .ForMember(crm => crm.CreatedOn, opt => opt.MapFrom(dto => dto.CreatedAt))
                .ForMember(crm => crm.ModifiedOn, opt => opt.MapFrom(dto => dto.UpdatedAt))
                .ForMember(crm => crm.ParentAccountId, opt => opt.MapFrom(dto => dto.ParentAccountId))
                .ForMember(crm => crm.IndustryCode, opt => opt.MapFrom(dto => dto.IndustryCode))
                .AfterMap((dto, crm) =>
                {
                    if (crm.Id == Guid.Empty)
                        crm.Id = Guid.NewGuid();

                    if (crm.ParentAccountId != null)
                        crm.ParentAccountId.LogicalName = Account.EntityLogicalName;
                });

            Mapper.CreateMap<Contact, ContactDto>()
                .ForMember(dto => dto.CreatedAt, opt => opt.MapFrom(crm => (DateTimeOffset?)crm.CreatedOn))
                .ForMember(dto => dto.UpdatedAt, opt => opt.MapFrom(crm => (DateTimeOffset?)crm.ModifiedOn))
                .ReverseMap()
                .ForMember(crm => crm.CreatedOn, opt => opt.MapFrom(dto => dto.CreatedAt))
                .ForMember(crm => crm.ModifiedOn, opt => opt.MapFrom(dto => dto.UpdatedAt));

            Mapper.CreateMap<Task, ActivityDto>()
                .ForMember(dto => dto.CreatedAt, opt => opt.MapFrom(crm => (DateTimeOffset?)crm.CreatedOn))
                .ForMember(dto => dto.UpdatedAt, opt => opt.MapFrom(crm => (DateTimeOffset?)crm.ModifiedOn))
                .ReverseMap()
                .ForMember(crm => crm.CreatedOn, opt => opt.MapFrom(dto => dto.CreatedAt))
                .ForMember(crm => crm.ModifiedOn, opt => opt.MapFrom(dto => dto.UpdatedAt));

            Mapper.CreateMap<PhoneCall, ActivityDto>()
                .ForMember(dto => dto.CreatedAt, opt => opt.MapFrom(crm => (DateTimeOffset?)crm.CreatedOn))
                .ForMember(dto => dto.UpdatedAt, opt => opt.MapFrom(crm => (DateTimeOffset?)crm.ModifiedOn))
                .ReverseMap()
                .ForMember(crm => crm.CreatedOn, opt => opt.MapFrom(dto => dto.CreatedAt))
                .ForMember(crm => crm.ModifiedOn, opt => opt.MapFrom(dto => dto.UpdatedAt));

            Mapper.CreateMap<Appointment, ActivityDto>()
                .ForMember(dto => dto.CreatedAt, opt => opt.MapFrom(crm => (DateTimeOffset?)crm.CreatedOn))
                .ForMember(dto => dto.UpdatedAt, opt => opt.MapFrom(crm => (DateTimeOffset?)crm.ModifiedOn))
                .ReverseMap()
                .ForMember(crm => crm.CreatedOn, opt => opt.MapFrom(dto => dto.CreatedAt))
                .ForMember(crm => crm.ModifiedOn, opt => opt.MapFrom(dto => dto.UpdatedAt));
        }
    }
}

