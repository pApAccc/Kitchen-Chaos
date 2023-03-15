using Cysharp.Threading.Tasks;
using Mono.CSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class TestUniTask : MonoBehaviour
    {
        Runner winnner;
        Runner runner1;
        Runner runner2;
        CancellationTokenSource linkToken;
        private void Start()
        {
            runner1 = new Runner("Player1", 2);
            runner2 = new Runner("Player2", 1);
            linkToken = CancellationTokenSource.CreateLinkedTokenSource(runner1.cancellationTokenSource.Token, runner2.cancellationTokenSource.Token);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Test2(runner1);
                winnner = runner1;
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Test2(runner2);
                winnner = runner2;
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                runner1.CancelRun(linkToken);
            }

        }
        private async void Test()
        {
            var firstOne = UniTask.WaitUntil(() =>
            {
                return runner1.isReach;
            });
            var secondOne = UniTask.WaitUntil(() => runner2.isReach);

            await UniTask.WhenAny(firstOne, secondOne);
            print("胜利者是：" + winnner.name);
        }

        private async void Test2(Runner runner)
        {
            await runner.Run(linkToken).SuppressCancellationThrow();
        }
    }
    class Runner
    {
        public bool isReach;
        public string name;
        public float runTime;
        public CancellationTokenSource cancellationTokenSource;
        public Runner(string name, float runTime)
        {
            this.name = name;
            this.runTime = runTime;
            cancellationTokenSource = new CancellationTokenSource();
        }

        public async UniTask Run(CancellationTokenSource linkToken)
        {
            float time = 0;
            while (time < runTime)
            {
                time += Time.deltaTime;
                Debug.Log(name + " " + time);
                await UniTask.Yield(linkToken.Token);
            }
            isReach = true;
            Debug.Log(name + "到达终点");
        }

        public void CancelRun(CancellationTokenSource cancellationTokenSource)
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            // cancellationTokenSource = new CancellationTokenSource();
        }

    }

}

