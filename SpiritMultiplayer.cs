using Microsoft.Xna.Framework;
using SpiritMod.Mechanics.BoonSystem;
using SpiritMod.Mechanics.Trails;
using SpiritMod.NPCs.AuroraStag;
using SpiritMod.NPCs.ExplosiveBarrel;
using SpiritMod.NPCs.Tides.Tide;
using SpiritMod.Projectiles;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod
{
	public static class SpiritMultiplayer
	{
		private struct Wait
		{
			public Func<bool> Condition { get; set; }
			public Action Result { get; set; }
		}

		private static List<Wait> _waits = new List<Wait>();

		public static void Load()
		{
			Main.OnTick += OnTick;
		}

		public static void Unload()
		{
			_waits = null;
			Main.OnTick -= OnTick;
		}

		public static void OnTick()
		{
			for (int i = 0; i < _waits.Count; i++)
			{
				Wait wait = _waits[i];
				if (wait.Condition.Invoke())
				{
					wait.Result?.Invoke();
					_waits.RemoveAt(i--);
				}
			}
		}

		public static void WaitUntil(Func<bool> condition, Action whenTrue)
		{
			_waits.Add(new Wait() { Condition = condition, Result = whenTrue });
		}

		// This is deprecated, DO NOT USE IT. Only here for compatability until later stages when we decide to swap it out for the new one.
		public static ModPacket WriteToPacket(ModPacket packet, byte msg, params object[] param)
		{
			packet.Write(msg);

			for (int m = 0; m < param.Length; m++)
			{
				object obj = param[m];
				if (obj is bool) packet.Write((bool)obj);
				else if (obj is byte) packet.Write((byte)obj);
				else if (obj is int) packet.Write((int)obj);
				else if (obj is float) packet.Write((float)obj);
				else if (obj is double) packet.Write((double)obj);
				else if (obj is short) packet.Write((short)obj);
				else if (obj is ushort) packet.Write((ushort)obj);
				else if (obj is sbyte) packet.Write((sbyte)obj);
				else if (obj is uint) packet.Write((uint)obj);
				else if (obj is decimal) packet.Write((decimal)obj);
				else if (obj is long) packet.Write((long)obj);
				else if (obj is string) packet.Write((string)obj);
			}
			return packet;
		}

		public static ModPacket WriteToPacket(int capacity, MessageType type, Action<ModPacket> beforeSend)
		{
			ModPacket packet = SpiritMod.Instance.GetPacket(capacity);
			packet.Write((byte)type);
			beforeSend?.Invoke(packet);
			return packet;
		}

		public static ModPacket WriteToPacketAndSend(int capacity, MessageType type, Action<ModPacket> beforeSend)
		{
			var packet = WriteToPacket(capacity, type, beforeSend);
			packet.Send();
			return packet;
		}

		public static void HandlePacket(BinaryReader reader, int whoAmI)
		{
			MessageType id = (MessageType)reader.ReadByte();
			byte player;
			switch (id)
			{
				case MessageType.AuroraData:
					MyWorld.auroraType = reader.ReadInt32();
					break;
				case MessageType.ProjectileData:
					GlyphProj.ReceiveProjectileData(reader, whoAmI);
					break;
				case MessageType.Dodge:
					player = reader.ReadByte();
					byte type = reader.ReadByte();
					if (Main.netMode == NetmodeID.Server)
					{
						ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.Dodge, 2);
						packet.Write(player);
						packet.Write(type);
						packet.Send(-1, whoAmI);
					}
					if (type == 1)
						Items.Glyphs.VeilGlyph.Block(Main.player[player]);
					else
						SpiritMod.Instance.Logger.Error("Unknown message (2:" + type + ")");
					break;
				case MessageType.Dash:
					player = reader.ReadByte();
					DashType dash = (DashType)reader.ReadByte();
					sbyte dir = reader.ReadSByte();
					if (Main.netMode == NetmodeID.Server)
					{
						ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.Dash, 3);
						packet.Write(player);
						packet.Write((byte)dash);
						packet.Write(dir);
						packet.Send(-1, whoAmI);
					}
					Main.player[player].GetModPlayer<MyPlayer>().PerformDash(dash, dir, false);
					break;
				case MessageType.PlayerGlyph:
					player = reader.ReadByte();
					GlyphType glyph = (GlyphType)reader.ReadByte();
					if (Main.netMode == NetmodeID.Server)
					{
						ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.PlayerGlyph, 2);
						packet.Write(player);
						packet.Write((byte)glyph);
						packet.Send(-1, whoAmI);
					}
					if (player == Main.myPlayer)
						break;
					Main.player[player].GetModPlayer<MyPlayer>().glyph = glyph;
					break;
				case MessageType.BossSpawnFromClient:
					if (Main.netMode == NetmodeID.Server)
					{
						player = reader.ReadByte();
						int bossType = reader.ReadInt32();
						int npcCenterX = reader.ReadInt32();
						int npcCenterY = reader.ReadInt32();

						if (NPC.AnyNPCs(bossType))
						{
							return;
						}

						WriteToPacket(SpiritMod.Instance.GetPacket(), (byte)MessageType.BossTitle, bossType).Send();
						int npcID = NPC.NewNPC(npcCenterX, npcCenterY, bossType);
						Main.npc[npcID].Center = new Vector2(npcCenterX, npcCenterY);
						Main.npc[npcID].netUpdate2 = true;
						NetMessage.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", Main.npc[npcID].GetTypeNetName()), new Color(175, 75, 255));
					}
					break;
				case MessageType.StartTide:
					TideWorld.TheTide = true;
					TideWorld.TideWaveIncrease();
					break;
				case MessageType.TideData:
					TideWorld.HandlePacket(reader);
					break;
				case MessageType.TameAuroraStag:
					int stagWhoAmI = reader.ReadInt32();

					if (Main.netMode == NetmodeID.Server)
						WriteToPacket(SpiritMod.Instance.GetPacket(4), (byte)MessageType.TameAuroraStag, stagWhoAmI).Send();

					(Main.npc[stagWhoAmI].modNPC as AuroraStag).TameAnimationTimer = AuroraStag.TameAnimationLength;
					break;
				case MessageType.SpawnTrail:
					int projindex = reader.ReadInt32();

					if (Main.netMode == NetmodeID.Server)
					{ //if received by the server, send to all clients instead
						WriteToPacket(SpiritMod.Instance.GetPacket(), (byte)MessageType.SpawnTrail, projindex).Send();
						break;
					}

					IManualTrailProjectile trailproj = (Main.projectile[projindex].modProjectile as IManualTrailProjectile);
					if (trailproj != null)
						trailproj.DoTrailCreation(SpiritMod.TrailManager);

					break;
				case MessageType.PlaceSuperSunFlower:
					MyWorld.superSunFlowerPositions.Add(new Point16(reader.ReadUInt16(), reader.ReadUInt16()));
					break;
				case MessageType.DestroySuperSunFlower:
					MyWorld.superSunFlowerPositions.Remove(new Point16(reader.ReadUInt16(), reader.ReadUInt16()));
					break;
				case MessageType.BossTitle:
					if (Main.netMode == NetmodeID.Server)
					{ //if received by the server, send to all clients instead
						WriteToPacket(SpiritMod.Instance.GetPacket(), (byte)MessageType.BossTitle, reader.ReadInt32()).Send();
						break;
					}
					BossTitles.SetNPCType(reader.ReadInt32());
					break;
				case MessageType.SpawnExplosiveBarrel: // this packet is only meant to be received by the server
					NPC.NewNPC(reader.ReadInt32(), reader.ReadInt32(), ModContent.NPCType<ExplosiveBarrel>(), 0, 2, 1, 0, 0); // gets forwarded to all clients
					break;
				case MessageType.StarjinxData:

					break;
				case MessageType.BoonData:
					ushort npcType = reader.ReadUInt16();
					ushort index = reader.ReadUInt16();
					byte boonType = reader.ReadByte();
					Boon ret = Activator.CreateInstance(BoonLoader.LoadedBoonTypes[boonType]) as Boon;
					ret.npc = Main.npc[index];
					SpiritMod.Instance.Logger.Debug($"received new boon data, index: {index} boonType: {boonType} which is {BoonLoader.LoadedBoonTypes[boonType].Name}");
					// wait until the npc at the specified index becomes the type we expect it to be, then set the boons up
					WaitUntil(() => Main.npc[index].type == (int)npcType, () =>
					{
						Main.npc[index].GetGlobalNPC<BoonNPC>().currentBoon = ret;
						Main.npc[index].GetGlobalNPC<BoonNPC>().currentBoon.SpawnIn();
						Main.npc[index].GetGlobalNPC<BoonNPC>().currentBoon.SetStats();
						SpiritMod.Instance.Logger.Debug($"current boon is now: {Main.npc[index].GetGlobalNPC<BoonNPC>().currentBoon.GetType().Name}");
					});
					break;
				default:
					SpiritMod.Instance.Logger.Error("Unknown net message (" + id + ")");
					break;
			}
		}
	}
}
