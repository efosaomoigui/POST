//using AutoMapper;
//using GIGLS.Core;
//using GIGLS.Core.Domain;
//using GIGLS.Core.DTO.Client;
//using GIGLS.Core.IServices.Client;
//using GIGLS.Infrastructure;
//using Microsoft.Owin.Security.DataHandler.Encoder;
//using System;
//using System.Collections.Generic;
//using System.Security.Cryptography;
//using System.Threading.Tasks;

//namespace GIGLS.Services.Implementation.Client
//{
//    public class ClientNodeService : IClientNodeService
//    {
//        private readonly IUnitOfWork _uow;

//        public ClientNodeService(IUnitOfWork uow)
//        {
//            _uow = uow;
//            MapperConfig.Initialize();
//        }

//        public async Task<IEnumerable<ClientNodeDTO>> GetClientNodes()
//        {
//            var clientNodes = await _uow.ClientNode.GetClientNodesAsync();
//            return clientNodes;
//        }

//        public async Task<ClientNodeDTO> GetClientNodeById(int clientNodeId)
//        {
//            var clientNode = await _uow.ClientNode.GetAsync(clientNodeId);

//            if (clientNode == null)
//            {
//                throw new GenericException("CLIENT_NODE_DOES_NOT_EXIST");
//            }
//            return Mapper.Map<ClientNodeDTO>(clientNode);
//        }

//        public async Task<object> AddClientNode(ClientNodeDTO clientNodeDto)
//        {
//            clientNodeDto.Name = clientNodeDto.Name?.Trim();
//            var name = clientNodeDto.Name?.ToLower();

//            if (await _uow.ClientNode.ExistAsync(v => v.Name.ToLower() == name))
//            {
//                throw new GenericException($"Client Node {clientNodeDto.Name} already exist");
//            }

//            var clientNodeId = Guid.NewGuid().ToString("N");

//            var key = new byte[32];
//            RNGCryptoServiceProvider.Create().GetBytes(key);
//            var base64Secret = TextEncodings.Base64Url.Encode(key);

//            var newClientNode = new ClientNode
//            {
//                ClientNodeId = clientNodeId,
//                Name = clientNodeDto.Name,
//                Base64Secret = base64Secret,
//                Status = clientNodeDto.Status
//            };

//            _uow.ClientNode.Add(newClientNode);
//            await _uow.CompleteAsync();
//            return new { id = newClientNode.ClientNodeId };
//        }

//        public async Task UpdateClientNode(int clientNodeId, ClientNodeDTO clientNodeDto)
//        {
//            var clientNode = await _uow.ClientNode.GetAsync(clientNodeId);

//            if (clientNode == null)
//            {
//                throw new GenericException("CLIENT_NODE_DOES_NOT_EXIST");
//            }
//            clientNode.Name = clientNodeDto.Name?.Trim();
//            clientNode.Base64Secret = clientNodeDto.Base64Secret;
//            clientNode.Status = clientNode.Status;
//            await _uow.CompleteAsync();
//        }

//        public async Task RemoveClientNode(int clientNodeId)
//        {
//            var clientNode = await _uow.ClientNode.GetAsync(clientNodeId);

//            if (clientNode == null)
//            {
//                throw new GenericException("CLIENT_NODE_DOES_NOT_EXIST");
//            }
//            _uow.ClientNode.Remove(clientNode);
//            await _uow.CompleteAsync();
//        }
//    }
//}
