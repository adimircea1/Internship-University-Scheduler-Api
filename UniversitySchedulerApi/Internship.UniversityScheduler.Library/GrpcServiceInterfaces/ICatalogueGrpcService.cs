using Internship.UniversityScheduler.Library.DataContracts;
using OnEntitySharedLogic.GRPC.GenericContracts;
using OnEntitySharedLogic.GRPC.Utils;
using ProtoBuf.Grpc.Configuration;

namespace Internship.UniversityScheduler.Library.GrpcServiceInterfaces;

[Service]
[ServiceTarget(SystemType = SystemType.UniversityScheduler)]
public interface ICatalogueGrpcService
{
    [Operation]
    public ValueTask<CatalogueDataContract> GetCatalogueDataAsync(SimpleValueContract<int> catalogueId);

    [Operation]
    public ValueTask<CatalogueDataContract> GetCatalogueDataByGroupAsync(SimpleValueContract<int> groupId);
}