﻿using Inputs;
using LevelMechanics;
using LevelMechanics.Settings;
using LevelMechanics.UI;
using MazeMechanics.Cells;
using MazeMechanics.Random;
using MazeMechanics.Storage;
using TinyIoC;

namespace MazeMechanics
{
    public class LevelMechanicsDiContainerPopulator : DiContainerPopulator
    {
        public override void RegisterDependencies(TinyIoCContainer container)
        {
            container.Register<IInput, InputBridge>().AsSingleton();
            container.Register<PlayerPresenter>().AsSingleton();
            
            RegisterLevelFlow(container);
            RegisterMazeBuilding(container);
            RegisterCollectables(container);
            
            RegisterViewPresenters(container);
            
            RegisterSettings(container);

            container.Register<IScoreStorage, PlayerPrefsStorage>().AsSingleton();
            container.Register<IRandomProvider, RandomProvider>().AsSingleton();
        }

        public override void RegisterMonobehListeners(MonoBehaviourMethodsCaller monoBehCaller)
        {
            monoBehCaller.Register(DI.Game.Resolve<LevelTimer>());
            monoBehCaller.Register(DI.Game.Resolve<CollectableRefresher>());
        }

        private static void RegisterLevelFlow(TinyIoCContainer container)
        {
            container.Register<LevelTimer>().AsSingleton();
            container.Register<ILevelTimeProvider>((c, p) => c.Resolve<LevelTimer>());
            container.Register<LevelStateDispatcher>().AsSingleton();
            container.Register<ILevelStateInfoProvider>((c, p) => c.Resolve<LevelStateDispatcher>());
            container.Register<ILevelStateInfoChanger>((c, p) => c.Resolve<LevelStateDispatcher>());
        }

        private static void RegisterMazeBuilding(TinyIoCContainer container)
        {
            container.Register<CellController>().AsSingleton();
            container.Register<MazeBuilder>().AsSingleton();
            container.Register<MazeCellPresenter>();
            container.Register<ITreasureGenerator, TreasureGenerator>();
        }

        private static void RegisterCollectables(TinyIoCContainer container)
        {
            container.Register<CollectableManager>().AsSingleton();
            container.Register<ICollectableManager>((c, p) => c.Resolve<CollectableManager>());
            container.Register<ICollectablePresenterFactory>((c, p) => c.Resolve<CollectableManager>());
            container.Register<CollectableRefresher>().AsSingleton();
        }

        private static void RegisterViewPresenters(TinyIoCContainer container)
        {
            container.Register<LevelInfoPresenter>().AsSingleton();
            container.Register<CoinsPresenter>().AsSingleton();
            container.Register<LevelTimePresenter>().AsSingleton();
        }

        private static void RegisterSettings(TinyIoCContainer container)
        {
            container.Register<SettingsProvider>().AsSingleton();
            container.Register<ICollectableSpawnSettingsProvider>((c, p) => c.Resolve<SettingsProvider>());
            container.Register<ITreasureGenerationSettingsProvider>((c, p) => c.Resolve<SettingsProvider>());
            container.Register<ITimeSettingsProvider>((c, p) => c.Resolve<SettingsProvider>());
        }
    }
}