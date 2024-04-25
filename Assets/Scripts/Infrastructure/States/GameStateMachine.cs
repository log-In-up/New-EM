using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.ServicesLocator;
using Assets.Scripts.Infrastructure.Services.PauseAndContinue;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.SaveLoad;
using Assets.Scripts.Infrastructure.Services.UserInterface;
using Assets.Scripts.StaticData;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Infrastructure.States
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IServiceLocator _serviceLocator;
        private IExitableState _activeState;
        private Dictionary<Type, IExitableState> _states;

        public GameStateMachine(ISceneLoader sceneLoader, IServiceLocator serviceLocator)
        {
            _sceneLoader = sceneLoader;
            _serviceLocator = serviceLocator;
        }

        ~GameStateMachine()
        {
            _states.Clear();
            _states = null;

            _activeState = null;
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();

            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();

            state.Enter(payload);
        }

        public void InitializeStateMashine()
        {
            IPersistentProgressService progressService = _serviceLocator.GetService<IPersistentProgressService>();
            IGameUI gameUI = _serviceLocator.GetService<IGameUI>();

            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(GameLoopState)] = new GameLoopState(this,
                    gameUI,
                    _serviceLocator.GetService<IPauseContinueService>()),
                [typeof(LoadLevelState)] = new LoadLevelState(this,
                    _serviceLocator.GetService<IGameFactory>(),
                    gameUI, progressService, _sceneLoader,
                    _serviceLocator.GetService<IStaticDataService>()),
                [typeof(LoadProgressState)] = new LoadProgressState(this,
                    gameUI, progressService,
                    _serviceLocator.GetService<ISaveLoadService>(),
                    _sceneLoader),
                [typeof(PreGameLoopState)] = new PreGameLoopState(this)
            };
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}