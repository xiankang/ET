﻿using System;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
	/// <summary>
	/// gate session 拦截器，收到的actor消息直接转发给客户端
	/// </summary>
	[ActorInterceptTypeHandler(AppType.Gate, ActorInterceptType.GateSession)]
	public class GateSessionActorInterceptInterceptTypeHandler : IActorInterceptTypeHandler
	{
		public async Task Handle(Session session, Entity entity, IActorMessage actorMessage)
		{
			ActorResponse actorResponse = new ActorResponse
			{
				RpcId = actorMessage.RpcId
			};
			try
			{
				// 发送给客户端
				Session clientSession = entity as Session;
				actorMessage.ActorId = 0;
				clientSession.Send(actorMessage);

				session.Reply(actorResponse);
				await Task.CompletedTask;
			}
			catch (Exception e)
			{
				actorResponse.Error = ErrorCode.ERR_SessionActorError;
				actorResponse.Message = $"session actor error {e}";
				session.Reply(actorResponse);
				throw;
			}
		}
	}
}
