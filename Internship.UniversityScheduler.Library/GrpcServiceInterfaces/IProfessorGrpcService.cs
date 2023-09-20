using Internship.UniversityScheduler.Library.DataContracts;
using OnEntitySharedLogic.GRPC.Utils;
using ProtoBuf.Grpc.Configuration;

namespace Internship.UniversityScheduler.Library.GrpcServiceInterfaces;

[Service]
[ServiceTarget(SystemType = SystemType.UniversityScheduler)]
public interface IProfessorGrpcService
{
    [Operation]
    public ValueTask AddProfessorAsync(ProfessorInputDataContract professorData);
}