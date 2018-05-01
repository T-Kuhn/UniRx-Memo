using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class ObsTScript : MonoBehaviour
{

    void Start()
    {
        //SubscribeAgainTest();
        //TwoOnCompleteActions();
        //MergeTwoObservables();
        //MergeTwoObservables2();
        MergeHotObservables();
    }

    // Test: Subscribing again to a already completed observable.
    // Outcome: The Observable gets executed again just like when subscribed to the first time.
    private void SubscribeAgainTest()
    {
        var strObservable = GetStringObsA();

        strObservable
        .DoOnCompleted(() => Debug.Log("completed."))
        .DoOnError(_ => Debug.Log("error!"))
        .Subscribe(onNext: x => Debug.Log(x));

        strObservable
        .DoOnCompleted(() => Debug.Log("completed."))
        .DoOnError(_ => Debug.Log("error!"))
        .Subscribe(onNext: x => Debug.Log(x));
    }

    // Test: Attach two onCompleted Actions to the same Observable.
    // Outcome: Both get executed.
    private void TwoOnCompleteActions()
    {
        var strObservable = GetStringObsA();

        strObservable
        .DoOnCompleted(() => Debug.Log("completed."))
        .DoOnError(_ => Debug.Log("error!"))
        .Subscribe(onNext: x => Debug.Log(x), onCompleted: () => Debug.Log("second onCompleted."));
    }

    // Test: Merge two Observables together to create a new Observable.
    // Outcome: The merged Observable contains all OnNexts of the source Observables.
    private void MergeTwoObservables()
    {
        var strObservable = GetStringObsA();

        strObservable
            // Note: Since we are dealing with cold observables concat will have the same result as merge.
            .Concat(GetStringObsB())
            .DoOnCompleted(() => Debug.Log("completed."))
            .Subscribe(onNext: x => Debug.Log(x));
    }

    // Test: Merge two Observables together to create a new Observable.
    // Outcome: The merged Observable contains all OnNexts of the source Observables.
    private void MergeTwoObservables2()
    {
        var strObservable = GetStringObsA();

        strObservable
            .Merge(GetStringObsB())
            .DoOnCompleted(() => Debug.Log("completed."))
            .Subscribe(onNext: x => Debug.Log(x));
    }

    private void MergeHotObservables()
    {
        // Generate values 0, 1, 2
        var s1 = Observable.Interval(TimeSpan.FromMilliseconds(250))
            .Take(3);

        // Generate values 10, 11, 12, 13, 14
        var s2 = Observable.Interval(TimeSpan.FromMilliseconds(150))
            .Take(5)
            .Select(i => i + 10);

        s1.Merge(s2).Subscribe(x => Debug.Log("result: " + x));
    }

    IObservable<string> GetStringObsA()
    {
        return Observable.Return<string>("string A");
    }

    IObservable<string> GetStringObsB()
    {
        return Observable.Return<string>("string B");
    }
}
