﻿using Small.Net.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using Vortice.Direct3D12;

namespace Small.Net.Graphic.D12.Core
{
    public class CommandQueueManager : IDisposable
    {
        private readonly IDisposableManager _disposableManager = new CommonDisposableManager();
        private readonly Queue<CommandAllocatorEntry> _commandAllocatorQueue = new Queue<CommandAllocatorEntry>();
        private readonly Queue<ID3D12GraphicsCommandList2> _commandListQueue = new Queue<ID3D12GraphicsCommandList2>();
        private readonly CommandListType _commandListType;
        private ID3D12Device5 _device;
        private ID3D12Fence _fence;
        private EventWaitHandle _fenceEvent;
        private long _fenceValue;


        private bool disposedValue;

        public CommandQueueManager(ID3D12Device5 device, CommandListType commandListType)
        {
            _fenceValue = 0;
            _commandListType = commandListType;
            _device = device;
            CommandQueue = device.CreateCommandQueue(new CommandQueueDescription
            {
                Type = commandListType,
                Flags = CommandQueueFlags.None,
                NodeMask = 0
            });
            _disposableManager.Add(CommandQueue);
            _fence = device.CreateFence(0, FenceFlags.None);
            _disposableManager.Add(_fence);
            _fenceEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
            _disposableManager.Add(_fenceEvent);
        }

        ~CommandQueueManager()
        {
            // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
            Dispose(disposing: false);
        }

        public ID3D12CommandQueue CommandQueue { get; private set; }

        DX12CommandList<ID3D12GraphicsCommandList2> GetCommandList()
        {
            ID3D12CommandAllocator allocator;
            if (_commandAllocatorQueue.Count != 0 && IsFenceComplete(_commandAllocatorQueue.Peek().FenceValue))
            {
                allocator = _commandAllocatorQueue.Dequeue().CommandAllocator;
                allocator.Reset();
            }
            else
            {
                allocator = CreateCommandAllocator();
            }

            ID3D12GraphicsCommandList2 commandList;
            if (_commandListQueue.Count != 0)
            {
                commandList = _commandListQueue.Dequeue();
                commandList.Reset(allocator);
            }
            else
            {
                commandList = CreateCommandList(allocator);
            }

            return new DX12CommandList<ID3D12GraphicsCommandList2>()
            {
                CommandList = commandList,
                CommandAllocator = allocator
            };
        }

        public long ExecuteCommandList(DX12CommandList<ID3D12GraphicsCommandList2> commandList)
        {
            commandList.CommandList.Close();
            CommandQueue.ExecuteCommandLists(commandList.CommandList);
            var fenceSignal = Signal();
            _commandAllocatorQueue.Enqueue(new CommandAllocatorEntry()
            {
                CommandAllocator = commandList.CommandAllocator,
                FenceValue = fenceSignal
            });
            _commandListQueue.Enqueue(commandList.CommandList);
            return fenceSignal;
        }

        public long Signal()
        {
            long fenceValue = ++_fenceValue;
            CommandQueue.Signal(_fence, fenceValue);
            return fenceValue;
        }

        public bool IsFenceComplete(long fenceValue)
        {
            return _fence.CompletedValue >= fenceValue;
        }

        public void WaitForFenceValue(long fenceValue)
        {
            if(!IsFenceComplete(fenceValue))
            {
                _fence.SetEventOnCompletion(fenceValue, _fenceEvent);
                _fenceEvent.WaitOne();
            }
        }

        public void Flush()
        {
            WaitForFenceValue(Signal());
        }

        public void Dispose()
        {
            // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected ID3D12CommandAllocator CreateCommandAllocator()
        {
            var allocator = _device.CreateCommandAllocator(_commandListType);
            _disposableManager.Add(allocator);
            return allocator;
        }

        protected ID3D12GraphicsCommandList2 CreateCommandList(ID3D12CommandAllocator allocator)
        {
            var cmdList =  _device.CreateCommandList(0, _commandListType, allocator);
            _disposableManager.Add(cmdList);
            var cmdList2 = cmdList.QueryInterface<ID3D12GraphicsCommandList2>();
            _disposableManager.Add(cmdList2);
            return cmdList2;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // supprimer l'état managé (objets managés)
                }

                // libérer les ressources non managées (objets non managés) et substituer le finaliseur
                _commandListQueue.Clear();
                _commandAllocatorQueue.Clear();
                _disposableManager.ReverseDispose = true;
                _disposableManager.Dispose();
                // affecter aux grands champs une valeur null
                _fence = null;
                CommandQueue = null;
                _device = null;
                disposedValue = true;
            }
        }

    }
}
