using Internship.UniversityScheduler.Library.DataContracts;
using OnEntitySharedLogic.GRPC.GenericContracts;
using OnEntitySharedLogic.GRPC.Utils;
using ProtoBuf.Grpc.Configuration;

namespace Internship.UniversityScheduler.Library.GrpcServiceInterfaces;

[Service]
[ServiceTarget(SystemType = SystemType.UniversityScheduler)]
public interface IStudentGrpcService
{
    [Operation]
    public ValueTask<StudentDataContract> GetStudentDataAsync(SimpleValueContract<int> studentId);

    [Operation]
    public ValueTask AddStudentAsync(StudentInputDataContract studentData);
}