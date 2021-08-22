using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Material;
using SpiritMod.Items.Sets.BloodcourtSet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Critters
{
	public class DreamstrideWisp : SpiritNPC, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dreamstride Wisp");
			Main.npcFrameCount[npc.type] = 5;
		}

		public override void SetDefaults()
		{
			npc.width = 32;
			npc.height = 32;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 5;
			npc.dontCountMe = true;
			if (!Main.dedServ)
			{
				npc.HitSound = SoundID.NPCHit36.WithVolume(0.5f);
				npc.DeathSound = SoundID.NPCDeath39.WithVolume(0.5f);
			}
			Main.npcCatchable[npc.type] = true;
			npc.catchItem = (short)ModContent.ItemType<DreamstrideEssence>();
			npc.knockBackResist = .45f;
			npc.aiStyle = 64;
			npc.npcSlots = 0;
			npc.noGravity = true;
			aiType = NPCID.Firefly;
			npc.alpha = 255;
			npc.scale = Main.rand.NextFloat(1.1f, 1.3f);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 Color.White * npc.Opacity, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			Texture2D circleGradient = mod.GetTexture("Effects/Masks/CircleGradient");
			spriteBatch.Draw(circleGradient, npc.Center - Main.screenPosition, null, Color.Red * 0.8f * npc.Opacity, 0, circleGradient.Size() / 2, new Vector2(0.33f, 0.45f) * npc.scale, SpriteEffects.None, 0);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (!Main.bloodMoon || !MyWorld.downedOccultist) 
				return 0f;

			return SpawnCondition.OverworldNight.Chance * 0.12f;
		}

		public override void AI()
		{
			ignorePlatforms = true;
			UpdateYFrame(7, 0, 4);
			npc.rotation = npc.velocity.X * 0.1f;
			Lighting.AddLight(npc.Center, npc.Opacity * Color.Red.ToVector3());
			if (!Main.dayTime)
				npc.alpha = (int)MathHelper.Max(npc.alpha - 3, 80);
			else
			{
				npc.alpha += 10;
				if(npc.alpha >= 255)
				{
					npc.active = false;
					npc.netUpdate = true;
				}
			}
		}

		public override void NPCLoot() => Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<DreamstrideEssence>());
	}
}
