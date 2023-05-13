using BepInEx;
using System;
using UnityEngine;
using Menu.Remix.MixedUI;
using static Player;
using Random = UnityEngine.Random;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using DevInterface;
using static MoreSlugcats.SingularityBomb;
using System.Drawing;
using Color = UnityEngine.Color;
using System.Collections.Generic;
using MoreSlugcats;

namespace DieButton;
[BepInPlugin("DieButton", "Die Button", "0.1")]

public class DieButton : BaseUnityPlugin{

    static bool _initialized;

    public static OptionsMenu optionsMenuInstance;

    private readonly float updateDelay = 50f;
    private float timeSinceLastUpdate = 0f;
    private bool firstUpdate = true;

    private readonly float deathDelay = 7f;
    private float deathTimer = 0f;
    private bool startedDeathTimer = false;

    private bool isdeathtrue = true;

    public FlareBomb fruit;
    public Creature realizedCreature;

    private void LogInfo(object ex) => Logger.LogInfo(ex);

    public void OnEnable(){
        LogInfo("This test is workin?");
        On.RainWorld.OnModsInit += RainWorld_OnModsInit;
    }

    private void RainWorld_OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self){
        orig(self);

        try
        {
            if (_initialized) return;
            _initialized = true;

            //Die Hooks
            On.Player.Update += OnPlayerDie;
            On.Player.Update += AllTheWaysToDie;
            On.Player.Update += StunDeath;
            On.Player.Update += RandomCrazyDie;
            On.Player.Update += RandomDie;
            //On.Player.Update += Test;
            On.Player.Update += NormalDie;


            //Sounds
            DieEnums.RegisterValues();

            //Remix Menu
            MachineConnector.SetRegisteredOI("DieButton", optionsMenuInstance = new OptionsMenu());

        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            Logger.LogMessage("WHOOPS something go wrong");
        }
        finally
        {
            orig.Invoke(self);
        }
    }

    private void OnPlayerDie(On.Player.orig_Update orig, Player self, bool eu)
    {
        orig(self, eu);

        var room = self.room;
        var pos = self.mainBodyChunk.pos;
        var color = self.ShortCutColor();

        if (!self.dead)
        {
            isdeathtrue = true;
        }

        if (OptionsMenu.OnlyOnDie.Value && self.dead && isdeathtrue)
        {
            int randomNumber = Random.Range(1, 6);
            switch (randomNumber)
            {
                case 1:
                    //Scavenger bomb
                    room?.AddObject(new Explosion(room, self, pos, 7, 250f, 6.2f, 2f, 280f, 0.25f, self, 0.7f, 160f, 1f));
                    room?.AddObject(new Explosion.ExplosionLight(pos, 280f, 1f, 7, color));
                    room?.AddObject(new Explosion.ExplosionLight(pos, 230f, 1f, 3, new Color(1f, 1f, 1f)));
                    room?.AddObject(new ExplosionSpikes(room, pos, 14, 30f, 9f, 7f, 170f, color));
                    room?.AddObject(new ShockWave(pos, 330f, 0.045f, 5, false));
                    room?.PlaySound(SoundID.Bomb_Explode, pos);

                    self.Die();
                    isdeathtrue = false;
                    break;

                case 2:
                    //Singularity bomb
                    room?.AddObject(new SparkFlash(pos, 300f, new Color(0f, 0f, 1f)));
                    room?.AddObject(new Explosion(room, self, pos, 7, 250f, 6.2f, 200f, 280f, 0.25f, self, 0.7f, 160f, 1f));
                    room?.AddObject(new Explosion(room, self, pos, 7, 2000f, 4f, 220f, 400f, 0.25f, self, 0.3f, 200f, 1f));
                    room?.AddObject(new Explosion.ExplosionLight(pos, 280f, 1f, 7, color));
                    room?.AddObject(new Explosion.ExplosionLight(pos, 230f, 1f, 3, new Color(1f, 1f, 1f)));
                    room?.AddObject(new Explosion.ExplosionLight(pos, 2000f, 2f, 60, color));
                    room?.AddObject(new ShockWave(pos, 350f, 0.485f, 300, highLayer: true));
                    room?.AddObject(new ShockWave(pos, 2000f, 0.185f, 180));
                    room?.PlaySound(SoundID.Bomb_Explode, pos);
                    room?.InGameNoise(new Noise.InGameNoise(pos, 9000f, self, 1f));

                    self.Die();
                    isdeathtrue = false;
                    break;
                case 3:
                    //FlareBomb
                    var flarebomb = new AbstractConsumable(self.room.world, AbstractPhysicalObject.AbstractObjectType.FlareBomb, fruit, self.room.GetWorldCoordinate(pos), self.room.game.GetNewID(), 0, 0, self.room.roomSettings.placedObjects[0].data as PlacedObject.ConsumableObjectData);
                    self.room.abstractRoom.AddEntity(flarebomb);
                    flarebomb.RealizeInRoom();

                    room?.AddObject(new Explosion(room, self, pos, 1, 250f, 0f, 2f, 0f, 0f, self, 0f, 0f, 0f));

                    self.Die();
                    isdeathtrue = false;
                    break;

                case 4:
                    //Dumb Ways To Die
                    self.room.PlaySound(DieEnums.DWTD, self.firstChunk);

                    self.Die();
                    isdeathtrue = false;
                    break;

                case 5:
                    //Die and Rain
                    self.room.game.world.rainCycle.cycleLength = 0;
                    self.room.roomRain.globalRain.InitDeathRain();

                    self.Die();
                    isdeathtrue = false;
                    break;

                default:
                    self.Die();
                    isdeathtrue = false;
                    break;
            }
        }
    }

    private void AllTheWaysToDie(On.Player.orig_Update orig, Player self, bool eu)
    {
        orig(self, eu);

        var room = self?.room;
        var pos = self.mainBodyChunk.pos;
        var color = self.ShortCutColor();


        if (Input.GetKey(OptionsMenu.Die.Value) && (firstUpdate || timeSinceLastUpdate >= updateDelay) && OptionsMenu.AllTheWaysToDie.Value && self.room is not null && self is not null && !self.dead)
        {
            int randomNumber = Random.Range(1, 8);
            switch (randomNumber)
            {
                case 1:
                    //Scavenger bomb
                    room?.AddObject(new Explosion(room, self, pos, 7, 250f, 6.2f, 2f, 280f, 0.25f, self, 0.7f, 160f, 1f));
                    room?.AddObject(new Explosion.ExplosionLight(pos, 280f, 1f, 7, color));
                    room?.AddObject(new Explosion.ExplosionLight(pos, 230f, 1f, 3, new Color(1f, 1f, 1f)));
                    room?.AddObject(new ExplosionSpikes(room, pos, 14, 30f, 9f, 7f, 170f, color));
                    room?.AddObject(new ShockWave(pos, 330f, 0.045f, 5, false));
                    room?.PlaySound(SoundID.Bomb_Explode, pos);

                    self.Die();

                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;

                case 2:
                    //Singularity bomb
                    room?.AddObject(new SparkFlash(pos, 300f, new Color(0f, 0f, 1f)));
                    room?.AddObject(new Explosion(room, self, pos, 7, 250f, 6.2f, 2f, 280f, 0.25f, self, 0.7f, 160f, 1f));
                    room?.AddObject(new Explosion(room, self, pos, 7, 2000f, 4f, 0f, 400f, 0.25f, self, 0.3f, 200f, 1f));
                    room?.AddObject(new Explosion.ExplosionLight(pos, 280f, 1f, 7, color));
                    room?.AddObject(new Explosion.ExplosionLight(pos, 230f, 1f, 3, new Color(1f, 1f, 1f)));
                    room?.AddObject(new Explosion.ExplosionLight(pos, 2000f, 2f, 60, color));
                    room?.AddObject(new ShockWave(pos, 350f, 0.485f, 300, highLayer: true));
                    room?.AddObject(new ShockWave(pos, 2000f, 0.185f, 180));
                    room?.PlaySound(SoundID.Bomb_Explode, pos);
                    room?.InGameNoise(new Noise.InGameNoise(pos, 9000f, self, 1f));

                    self.Die();

                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;
                case 3:
                    //FlareBomb
                    var flarebomb = new AbstractConsumable(self.room.world, AbstractPhysicalObject.AbstractObjectType.FlareBomb, fruit, self.room.GetWorldCoordinate(pos), self.room.game.GetNewID(), 0, 0, self.room.roomSettings.placedObjects[0].data as PlacedObject.ConsumableObjectData);
                    self.room.abstractRoom.AddEntity(flarebomb);
                    flarebomb.RealizeInRoom();

                    room?.AddObject(new Explosion(room, self, pos, 1, 250f, 0f, 2f, 0f, 0f, self, 0f, 0f, 0f));

                    self.Die();

                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;

                case 4:
                    //Dumb Ways To Die
                    self.room.PlaySound(DieEnums.DWTD, self.firstChunk);

                    self.Die();

                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;

                case 5:
                    //Die and Rain
                    self.room.game.world.rainCycle.cycleLength = 0;
                    self.room.roomRain.globalRain.InitDeathRain();

                    self.Die();

                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;

                case 6:
                    //Random creature and then die

                    self.Stun(1);

                    var creatureTypes = new List<CreatureTemplate.Type>()
                    {
                        CreatureTemplate.Type.PinkLizard, CreatureTemplate.Type.GreenLizard,
                        CreatureTemplate.Type.BlueLizard, CreatureTemplate.Type.YellowLizard,
                        CreatureTemplate.Type.WhiteLizard, CreatureTemplate.Type.RedLizard,
                        CreatureTemplate.Type.BlackLizard, CreatureTemplate.Type.Salamander,
                        CreatureTemplate.Type.Vulture, CreatureTemplate.Type.BigSpider,
                        /*CreatureTemplate.Type.Spider, */CreatureTemplate.Type.KingVulture,
                        CreatureTemplate.Type.DaddyLongLegs, CreatureTemplate.Type.DropBug,
                        CreatureTemplate.Type.BrotherLongLegs, CreatureTemplate.Type.MirosBird,
                        CreatureTemplate.Type.Centipede, CreatureTemplate.Type.RedCentipede,
                        CreatureTemplate.Type.SpitterSpider,

                            MoreSlugcatsEnums.CreatureTemplateType.EelLizard, MoreSlugcatsEnums.CreatureTemplateType.MotherSpider,
                            MoreSlugcatsEnums.CreatureTemplateType.HunterDaddy, MoreSlugcatsEnums.CreatureTemplateType.ZoopLizard,
                            MoreSlugcatsEnums.CreatureTemplateType.SpitLizard, MoreSlugcatsEnums.CreatureTemplateType.TrainLizard,
                            MoreSlugcatsEnums.CreatureTemplateType.TerrorLongLegs,
                    };

                    int randomCreature = Random.Range(0, creatureTypes.Count);

                    var creature = new AbstractCreature(self.room.world, StaticWorld.GetCreatureTemplate(creatureTypes[randomCreature]), realizedCreature, self.room.GetWorldCoordinate(pos), self.room.game.GetNewID());
                    creature.RealizeInRoom();

                    //Spiders drink a monster
                    #region Spider For (deprecated)
                    //if (creatureTypes.Contains(CreatureTemplate.Type.Spider))
                    //{
                    //    for (int i = 0; i < 20; i++)
                    //    {
                    //        creature.RealizeInRoom();
                    //    }
                    //}
                    #endregion
                    //Spiders drink a monster

                    startedDeathTimer = true;
                    timeSinceLastUpdate = 0f;
                    firstUpdate = false;
                    break;

                case 7:
                    //Die and ascension
                    if (self.room.game.IsStorySession)
                    {
                        self.room.game.GetStorySession.saveState.deathPersistentSaveData.ascended = true;
                        self.room.game.manager.RequestMainProcessSwitch(ProcessManager.ProcessID.Statistics, 5f);
                        self.room.game.GetStorySession.saveState.AppendCycleToStatistics(self, self.room.game.GetStorySession, death: true, 0);
                        self.room.game.GoToRedsGameOver();
                    }
                    self.Die();

                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;

                default:
                    self.Die();
                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;
            }
            timeSinceLastUpdate = 0.0f;
            firstUpdate = false;
        }
    }

    private void StunDeath(On.Player.orig_Update orig, Player self, bool eu)
    {
        orig(self, eu);

        var room = self.room;
        var pos = self.mainBodyChunk.pos;
        var color = self.ShortCutColor();

        if (OptionsMenu.StunDeath.Value && self.Stunned && self.room is not null && self is not null && !self.dead)
        {
            int randomNumber = Random.Range(1, 6);
            switch (randomNumber)
            {
                case 1:
                    //Scavenger bomb
                    room?.AddObject(new Explosion(room, self, pos, 7, 250f, 6.2f, 2f, 280f, 0.25f, self, 0.7f, 160f, 1f));
                    room?.AddObject(new Explosion.ExplosionLight(pos, 280f, 1f, 7, color));
                    room?.AddObject(new Explosion.ExplosionLight(pos, 230f, 1f, 3, new Color(1f, 1f, 1f)));
                    room?.AddObject(new ExplosionSpikes(room, pos, 14, 30f, 9f, 7f, 170f, color));
                    room?.AddObject(new ShockWave(pos, 330f, 0.045f, 5, false));
                    room?.PlaySound(SoundID.Bomb_Explode, pos);

                    self.Die();

                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;

                case 2:
                    //Singularity bomb
                    room?.AddObject(new SparkFlash(pos, 300f, new Color(0f, 0f, 1f)));
                    room?.AddObject(new Explosion(room, self, pos, 7, 250f, 6.2f, 2f, 280f, 0.25f, self, 0.7f, 160f, 1f));
                    room?.AddObject(new Explosion(room, self, pos, 7, 2000f, 4f, 0f, 400f, 0.25f, self, 0.3f, 200f, 1f));
                    room?.AddObject(new Explosion.ExplosionLight(pos, 280f, 1f, 7, color));
                    room?.AddObject(new Explosion.ExplosionLight(pos, 230f, 1f, 3, new Color(1f, 1f, 1f)));
                    room?.AddObject(new Explosion.ExplosionLight(pos, 2000f, 2f, 60, color));
                    room?.AddObject(new ShockWave(pos, 350f, 0.485f, 300, highLayer: true));
                    room?.AddObject(new ShockWave(pos, 2000f, 0.185f, 180));
                    room?.PlaySound(SoundID.Bomb_Explode, pos);
                    room?.InGameNoise(new Noise.InGameNoise(pos, 9000f, self, 1f));

                    self.Die();

                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;
                case 3:
                    //FlareBomb
                    var flarebomb = new AbstractConsumable(self.room.world, AbstractPhysicalObject.AbstractObjectType.FlareBomb, fruit, self.room.GetWorldCoordinate(pos), self.room.game.GetNewID(), 0, 0, self.room.roomSettings.placedObjects[0].data as PlacedObject.ConsumableObjectData);
                    self.room.abstractRoom.AddEntity(flarebomb);
                    flarebomb.RealizeInRoom();

                    room?.AddObject(new Explosion(room, self, pos, 1, 250f, 0f, 2f, 0f, 0f, self, 0f, 0f, 0f));

                    self.Die();

                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;

                case 4:
                    //Dumb Ways To Die
                    self.room.PlaySound(DieEnums.DWTD, self.firstChunk);

                    self.Die();

                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;

                case 5:
                    //Die and Rain
                    self.room.game.world.rainCycle.cycleLength = 0;
                    self.room.roomRain.globalRain.InitDeathRain();

                    self.Die();

                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;

                default:
                    self.Die();
                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;
            }
            timeSinceLastUpdate = 0.0f;
            firstUpdate = false;
        }
    }

    private void NormalDie(On.Player.orig_Update orig, Player self, bool eu)
    {
        orig(self, eu);

        var room = self.room;

        if (self.dead)
        {
            firstUpdate = true;
        }

        timeSinceLastUpdate += Time.deltaTime;

        if (Input.GetKey(OptionsMenu.Die.Value) && (firstUpdate || timeSinceLastUpdate >= updateDelay) && OptionsMenu.NormalDie.Value && self.room is not null && self is not null && !self.dead)
        {
            self.Die();
            timeSinceLastUpdate = 0.0f;
            firstUpdate = false;
        }
    }
    
    private void RandomDie(On.Player.orig_Update orig, Player self, bool eu)
    {
        orig(self, eu);

        var room = self?.room;
        var pos = self.mainBodyChunk.pos;
        var color = self.ShortCutColor();

        timeSinceLastUpdate += Time.deltaTime;

        if (startedDeathTimer) 
        { 
            deathTimer += Time.deltaTime; 
        }

        if (self.dead)
        {
            startedDeathTimer = false;
        }

        if (startedDeathTimer && deathTimer >= deathDelay && !self.dead)
        {
            self.Die();
            deathTimer = 0.0f;
            startedDeathTimer = false;
        }

        if (Input.GetKey(OptionsMenu.Die.Value) && (firstUpdate || timeSinceLastUpdate >= updateDelay) && OptionsMenu.RandomDie.Value && self.room is not null && self is not null && !self.dead)
        {
            int randomNumber = Random.Range(1, 10);
            switch (randomNumber)
            {
                case 1:
                    //Scavenger bomb
                    room?.AddObject(new Explosion(room, self, pos, 7, 250f, 6.2f, 2f, 280f, 0.25f, self, 0.7f, 160f, 1f));
                    room?.AddObject(new Explosion.ExplosionLight(pos, 280f, 1f, 7, color));
                    room?.AddObject(new Explosion.ExplosionLight(pos, 230f, 1f, 3, new Color(1f, 1f, 1f)));
                    room?.AddObject(new ExplosionSpikes(room, pos, 14, 30f, 9f, 7f, 170f, color));
                    room?.AddObject(new ShockWave(pos, 330f, 0.045f, 5, false));
                    room?.PlaySound(SoundID.Bomb_Explode, pos);

                    self.Die();

                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;

                case 2:
                    //Singularity bomb
                    room?.AddObject(new SparkFlash(pos, 300f, new Color(0f, 0f, 1f)));
                    room?.AddObject(new Explosion(room, self, pos, 7, 250f, 6.2f, 2f, 280f, 0.25f, self, 0.7f, 160f, 1f));
                    room?.AddObject(new Explosion(room, self, pos, 7, 2000f, 4f, 0f, 400f, 0.25f, self, 0.3f, 200f, 1f));
                    room?.AddObject(new Explosion.ExplosionLight(pos, 280f, 1f, 7, color));
                    room?.AddObject(new Explosion.ExplosionLight(pos, 230f, 1f, 3, new Color(1f, 1f, 1f)));
                    room?.AddObject(new Explosion.ExplosionLight(pos, 2000f, 2f, 60, color));
                    room?.AddObject(new ShockWave(pos, 350f, 0.485f, 300, highLayer: true));
                    room?.AddObject(new ShockWave(pos, 2000f, 0.185f, 180));
                    room?.PlaySound(SoundID.Bomb_Explode, pos);
                    room?.InGameNoise(new Noise.InGameNoise(pos, 9000f, self, 1f));

                    self.Die();

                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;
                case 3:
                    //FlareBomb
                    var flarebomb = new AbstractConsumable(self.room.world, AbstractPhysicalObject.AbstractObjectType.FlareBomb, fruit, self.room.GetWorldCoordinate(pos), self.room.game.GetNewID(), 0, 0, self.room.roomSettings.placedObjects[0].data as PlacedObject.ConsumableObjectData);
                    self.room.abstractRoom.AddEntity(flarebomb);
                    flarebomb.RealizeInRoom();

                    room?.AddObject(new Explosion(room, self, pos, 1, 250f, 0f, 2f, 0f, 0f, self, 0f, 0f, 0f));

                    self.Die();

                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;

                case 4:
                    //Dumb Ways To Die
                    self.room.PlaySound(DieEnums.DWTD, self.firstChunk);

                    self.Die();

                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;

                case 5:
                    //Die and Rain
                    self.room.game.world.rainCycle.cycleLength = 0;
                    self.room.roomRain.globalRain.InitDeathRain();

                    self.Die();

                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;

                case 6:
                    //Random creature and then die

                    self.Stun(1);

                    var creatureTypes = new List<CreatureTemplate.Type>()
                    {
                        CreatureTemplate.Type.PinkLizard, CreatureTemplate.Type.GreenLizard,
                        CreatureTemplate.Type.BlueLizard, CreatureTemplate.Type.YellowLizard,
                        CreatureTemplate.Type.WhiteLizard, CreatureTemplate.Type.RedLizard,
                        CreatureTemplate.Type.BlackLizard, CreatureTemplate.Type.Salamander,
                        CreatureTemplate.Type.Vulture, CreatureTemplate.Type.BigSpider,
                        /*CreatureTemplate.Type.Spider, */CreatureTemplate.Type.KingVulture,
                        CreatureTemplate.Type.DaddyLongLegs, CreatureTemplate.Type.DropBug,
                        CreatureTemplate.Type.BrotherLongLegs, CreatureTemplate.Type.MirosBird,
                        CreatureTemplate.Type.Centipede, CreatureTemplate.Type.RedCentipede,
                        CreatureTemplate.Type.SpitterSpider,

                            MoreSlugcatsEnums.CreatureTemplateType.EelLizard, MoreSlugcatsEnums.CreatureTemplateType.MotherSpider,
                            MoreSlugcatsEnums.CreatureTemplateType.HunterDaddy, MoreSlugcatsEnums.CreatureTemplateType.ZoopLizard,
                            MoreSlugcatsEnums.CreatureTemplateType.SpitLizard, MoreSlugcatsEnums.CreatureTemplateType.TrainLizard,
                            MoreSlugcatsEnums.CreatureTemplateType.TerrorLongLegs,
                    };

                    int randomCreature = Random.Range(0, creatureTypes.Count);

                    var creature = new AbstractCreature(self.room.world, StaticWorld.GetCreatureTemplate(creatureTypes[randomCreature]), realizedCreature, self.room.GetWorldCoordinate(pos), self.room.game.GetNewID());
                    creature.RealizeInRoom();

                    //Spiders drink a monster
                    #region Spider For (deprecated)
                    //if (creatureTypes.Contains(CreatureTemplate.Type.Spider))
                    //{
                    //    for (int i = 0; i < 20; i++)
                    //    {
                    //        creature.RealizeInRoom();
                    //    }
                    //}
                    #endregion
                    //Spiders drink a monster

                    startedDeathTimer = true;
                    timeSinceLastUpdate = 0f;
                    firstUpdate = false;
                    break;
                case 7:
                    self.room.PlaySound(DieEnums.rot_tor, self.firstChunk);

                    self.Die();

                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;

                case 8:
                    self.room.PlaySound(DieEnums.sleepy, self.firstChunk);

                    self.Die();

                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;

                case 9:
                    self.room.PlaySound(DieEnums.IDWTD, self.firstChunk);

                    self.Die();

                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;

                default:
                    self.Die();
                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;
            }
            timeSinceLastUpdate = 0.0f;
            firstUpdate = false;
        }
    }

    private void RandomCrazyDie(On.Player.orig_Update orig, Player self, bool eu)
    {
        orig(self, eu);

        var room = self.room;
        var pos = self.mainBodyChunk.pos;
        var color = self.ShortCutColor();

        timeSinceLastUpdate += Time.deltaTime;

        if (self.dead)
        {
            firstUpdate = true;
        }

        if (Input.GetKey(OptionsMenu.Die.Value) && (firstUpdate || timeSinceLastUpdate >= updateDelay) && OptionsMenu.RandomCrazyDie.Value && self.room is not null && self is not null)
        {
            int randomNumber = Random.Range(1, 4);
            switch (randomNumber)
            {
                case 1:
                    //Die and ascension
                    if (self.room.game.IsStorySession)
                    {
                        self.room.game.GetStorySession.saveState.deathPersistentSaveData.ascended = true;
                        self.room.game.manager.RequestMainProcessSwitch(ProcessManager.ProcessID.Statistics, 5f);
                        self.room.game.GetStorySession.saveState.AppendCycleToStatistics(self, self.room.game.GetStorySession, death: true, 0);
                        self.room.game.GoToRedsGameOver();
                    }
                    self.Die();

                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;

                default:
                    self.Die();

                    timeSinceLastUpdate = 0.0f;
                    firstUpdate = false;
                    break;
            }
            timeSinceLastUpdate = 0.0f;
            firstUpdate = false;
        }
    }
    
    /// <summary>
    /// DO NOT LEAVE THIS IN THE LAST BUILD
    /// </summary>
    //private void Test(On.Player.orig_Update orig, Player self, bool eu)
    //{
    //    orig(self, eu);

    //    if (Input.GetKey(OptionsMenu.Die.Value) && (firstUpdate || timeSinceLastUpdate >= updateDelay) && OptionsMenu.Test.Value && self.room is not null && self is not null)
    //    {

    //        int randomNumber = Random.Range(1, 2);

    //        switch (randomNumber)
    //        {
    //            case 1:
    //                //YOU IDEA
    //                break;

    //            default:
    //                self.Die();
    //                timeSinceLastUpdate = 0f;
    //                firstUpdate = false;
    //                break;
    //        }
    //        timeSinceLastUpdate = 0f;
    //        firstUpdate = false;
    //    }
    //}
    /// <summary>
    /// DO NOT LEAVE THIS IN THE LAST BUILD
    /// </summary>
}