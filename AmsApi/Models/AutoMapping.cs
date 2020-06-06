using AmsApi.Domain;
using AutoMapper;

namespace AmsApi.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Location, LocationModel>();
            CreateMap<LocationModel, Location>();

            CreateMap<ClosedDate, ClosedDateModel>();
            CreateMap<ClosedDateModel, ClosedDate>();

            CreateMap<ServiceCategory,CategoryModel>();
            CreateMap<CategoryModel, ServiceCategory>();

            CreateMap<Appointment, AppointmentModel>();
            CreateMap<AppointmentModel, Appointment>();

            CreateMap<ClientModel, ClientModel>();
            CreateMap<ClientModel, Client>();

            CreateMap<Staff, StaffModel>();
            CreateMap<StaffModel, Staff>();

            CreateMap<StaffWorkingHour, StaffWorkingHourModel>();
            CreateMap<StaffWorkingHourModel, StaffWorkingHour>();


            


        }
    }
}
