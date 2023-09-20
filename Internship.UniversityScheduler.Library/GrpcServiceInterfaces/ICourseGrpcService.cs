using Internship.UniversityScheduler.Library.DataContracts;
using OnEntitySharedLogic.GRPC.GenericContracts;
using OnEntitySharedLogic.GRPC.Utils;
using ProtoBuf.Grpc.Configuration;

namespace Internship.UniversityScheduler.Library.GrpcServiceInterfaces;

[Service]
[ServiceTarget(SystemType = SystemType.UniversityScheduler)]
public interface ICourseGrpcService
{
    [Operation]
    public ValueTask<CourseDataContract> GetCourseDataAsync(SimpleValueContract<int> courseId);
}