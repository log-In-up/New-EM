using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services;
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
        private readonly SceneLoader _sceneLoader;
        private readonly ServiceLocator _serviceLocator;
        private IExitableState _activeState;
        private Dictionary<Type, IExitableState> _states;

        public GameStateMachine(SceneLoader sceneLoader, ServiceLocator serviceLocator)
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
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(GameLoopState)] = new GameLoopState(this,
                    _serviceLocator.GetService<IGameUI>(),
                    _serviceLocator.GetService<IPauseContinueService>()),
                [typeof(LoadLevelState)] = new LoadLevelState(this,
                    _sceneLoader,
                    _serviceLocator.GetService<IGameFactory>(),
                    _serviceLocator.GetService<IPersistentProgressService>(),
                    _serviceLocator.GetService<IStaticDataService>(),
                    _serviceLocator.GetService<IGameUI>()),
                [typeof(LoadProgressState)] = new LoadProgressState(this,
                    _sceneLoader,
                    _serviceLocator.GetService<IPersistentProgressService>(),
                    _serviceLocator.GetService<ISaveLoadService>(),
                    _serviceLocator.GetService<IGameUI>()),
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