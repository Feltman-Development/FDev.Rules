using AutoMapper;

namespace FDEV.Rules.Demo.Domain.Mapping
{
    /// <summary>
    /// INFO: Example of a Automapper Profile implementation
    /// </summary>
    public class DTOMappingProfile : Profile
    {
        public DTOMappingProfile()
        {
            //CreateMap<DimensionDTO, Dimension>().ReverseMap();

            //CreateMap<MappingDTO, Mapping>()
            //     .ForMember(dest => dest.mappingId, opt => opt.Condition(src => src.MappingId != Guid.Empty));

            //CreateMap<Mapping, MappingDTO>()
            //    .ForMember(x => x.HasUnmappedAccounts, opt => opt.Ignore())
            //    .ForMember(x => x.UsedBy, opt => opt.Ignore())
            //    .ForMember(x => x.IsMappedToSummaryAccount, opt => opt.Ignore());

            //CreateMap<CompanyDTO, Company>()
            //    .ForMember(dest => dest.companyId, opt => opt.MapFrom(source => source.CompanyId))
            //    .ReverseMap();

            //CreateMap<AccountDTO, Account>()
            //    .ForMember(dest => dest.lvl, opt => opt.MapFrom(source => source.Level))
            //    .ForMember(dest => dest.rgt, opt => opt.MapFrom(source => source.Right))
            //    .ForMember(dest => dest.lft, opt => opt.MapFrom(source => source.Left))
            //    .ForMember(dest => dest.lft, opt => opt.MapFrom(source => source.Left))
            //    .ForMember(dest => dest.hasChild, opt => opt.MapFrom(source => source.HasChild))
            //    .ForMember(dest => dest.mappable, opt => opt.MapFrom(source => source.Mappable))
            //    .ForMember(dest => dest.sysAct, opt => opt.MapFrom(source => source.SysAccount))
            //    .ForMember(dest => dest.sysCode, opt => opt.MapFrom(source => source.SysCode))
            //    .ReverseMap();

            //CreateMap<AccountMapping, AccountMappingDTO>()
            //    .ForMember(x => x.KeyId, opt => opt.Ignore())
            //    .ForMember(dest => dest.IsIgnored, opt => opt.MapFrom(source => source.ignore))
            //    .ReverseMap();

            //CreateMap<CompanyDTO, Company>()
            //    .ForMember(dest => dest.customerId, opt => opt.Condition(src => src.CustomerId != Guid.Empty))
            //    .ForMember(dest => dest.companyId, opt => opt.Condition(src => src.CustomerId != Guid.Empty));

            //CreateMap<Company, CompanyDTO>()
            //    .ForMember(dest => dest.CustomerId, opt => opt.Ignore());

            //CreateMap<CountryDTO, Country>()
            //    .ForMember(x => x.countryCode, opt => opt.Ignore())
            //    .ReverseMap();

            //CreateMap<CurrencyDTO, Currency>()
            //    .ForMember(src => src.currencyId, opt => opt.MapFrom(dest => dest.Id))
            //    .ReverseMap();

            //CreateMap<CustomerDTO, Customer>()
            //    .ForMember(x => x.customerId, opt => opt.Ignore())
            //    .ReverseMap();

            //CreateMap<DimensionDTO, Dimension>()
            //    .ForMember(dest => dest.customerId, opt => opt.Condition(src => src.CustomerId != Guid.Empty))
            //    .ForMember(x => x.dimensionId, opt => opt.Ignore())
            //    .ReverseMap();

            //CreateMap<ElimGroupDTO, AccountElimGroup>()
            //    .ForMember(x => x.groupId, opt => opt.Ignore())
            //    .ReverseMap();

            //CreateMap<GroupAccountDTO, AccountDTO>()
            //    .ReverseMap();

            //CreateMap<BudgetDTO, Budget>()
            //    .ReverseMap();

            //CreateMap<PeriodDTO, Period>()
            //    .ReverseMap();

            //CreateMap<PeriodSourceDTO, PeriodSource>()
            //    .ForMember(dest => dest.year, opt => opt.MapFrom(source => source.Year))
            //    .ReverseMap();

            //CreateMap<SourceDTO, Source>()
            //.ReverseMap();

            //CreateMap<RoleDTO, Role>();

            //CreateMap<Role, RoleDTO>()
            //    .ForMember(dest => dest.Name, opt => opt.Ignore());
                

            //CreateMap<IEnumerable<Role>, RoleDTO>()
            //    .ReverseMap();

            //CreateMap<Konsolidator.Model.Administration.Rights.User, UserDTO>()
            //    .ForMember(dest => dest.Password, opt => opt.Ignore())
            //    .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.UserRoleList.FirstOrDefault()))
            //    .IncludeAllDerived();

            //CreateMap<UserDTO, Konsolidator.Model.Administration.Rights.User>()
            //    .ForMember(dest => dest.Password, opt => opt.Ignore())
            //    .ForMember(dest => dest.UserRoleList, opt => opt.MapFrom(src => new List<RoleDTO> { src.Role }));

            //CreateMap<ManualKPIDTO, ManualKPI>()
            //    .ForMember(dest => dest.KPIId, opt => opt.MapFrom(source => source.KPIId))
            //    .ForMember(x => x.KPIAccount, opt => opt.Ignore())
            //    .ForMember(x => x.CustomerId, opt => opt.Ignore());

            //CreateMap<ManualKPI, ManualKPIDTO>()
            //    .ForMember(x => x.CustomerId, opt => opt.Ignore());

            //CreateMap<SystemKPIDTO, KPIAbs>()
            //    .ReverseMap();

            //CreateMap<SystemKPIDTO, SystemKPI>()
            //    .ReverseMap();

            //CreateMap<UploadCompanyDTO, UploadCompanyModel>()
            //    .IncludeAllDerived()
            //    .ForMember(dest => dest.lastEditBy, opt => opt.AllowNull())
            //    .ReverseMap();

            //CreateMap<KPIUploadRecords, KpiUploadRecordDTO>()
            //    .ForMember(dest => dest.Kpi, opt => opt.MapFrom(source => source.KPI))
            //    .ReverseMap();

            //CreateMap<ManualKPI, KpiDTO>()
            //    .ReverseMap();

            //CreateMap<KPIUpload, KpiUploadDTO>()
            //    .ForMember(x => x.LegalName, opt => opt.Ignore())
            //    .ForMember(x => x.ShortName, opt => opt.Ignore())
            //    .ForMember(x => x.KpiValues, opt => opt.Condition(src => src.KPIUploadRecords != null))
            //    .ForMember(x => x.KpiValues, opt => opt.MapFrom(src => src.KPIUploadRecords.Select(kpi => new KpiValueDTO(kpi.KPI.Code, kpi.value))))
            //    .ReverseMap();

            //CreateMap<CountryDTO, Country>()
            //    .ReverseMap();

            //CreateMap<SelectorSourceDTO, Source>()
            //    .ReverseMap();


            //CreateMap<JournalDTO, Journal>()
            //    .ForMember(x => x.keyid, opt => opt.Ignore())
            //    .ForMember(dest => dest.journalId, opt => opt.MapFrom(src => src.Id))
            //    .ReverseMap();

            //CreateMap<JournalRecordDTO, JournalRecord>()
            //       .ForMember(x => x.keyid, opt => opt.Ignore())
            //       .ForMember(dest => dest.jrId, opt => opt.MapFrom(src => src.Id));

            //CreateMap<JournalRecord, JournalRecordDTO>()
            //       .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.jrId));

            //CreateMap<ExchangeRateDTO, ExchRate>()
            //    .ForMember(dest => dest.erId, opt => opt.MapFrom(src => src.Id))
            //    .ReverseMap();

            //CreateMap<ExchangeRateBudgetDTO, ExchRate>()
            //   .ReverseMap();

            //CreateMap<CashFlowAccountMappingDTO, CashFlowAccountMapping>()
            //    .ForMember(dest => dest.amId, opt => opt.MapFrom(src => src.Id))
            //    .ReverseMap();

            //CreateMap<ConsolidationStatusDTO, Consolidation>()
            //    .ReverseMap();

            //CreateMap<CashFlowConsolidationStatus, CashFlowConsolidationStatusDTO>()
            //    .ReverseMap();

            //CreateMap<SecurityGroup, SecurityGroupDTO>()
            //    .ReverseMap();

            //CreateMap<Structure, StructureDTO>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.structureId))
            //    .ForMember(dest => dest.CustomerId, opt => opt.Ignore());

            //CreateMap<StructureDTO, Structure>()
            //    .ForMember(dest => dest.structureId, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.customerId, opt => opt.Condition(src => src.CustomerId != Guid.Empty));
        }
    }
}
