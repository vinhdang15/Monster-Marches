using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UpdateAndDownload : MonoBehaviour
{
    private Action onFinishedCallback;

    public void CheckForUpdateAndDownload(Action action)
    {
        onFinishedCallback = action;
        Addressables.InitializeAsync().Completed += OnAddressableInittialized;
    }
    public void OnAddressableInittialized(AsyncOperationHandle<IResourceLocator> locator)
    {
        if(locator.Status == AsyncOperationStatus.Succeeded)
        {
            StartCoroutine(CheckForUpdateAndDownloadCoroutine());
        }
    }

    private IEnumerator CheckForUpdateAndDownloadCoroutine()
    {
        while(Caching.ready == false) yield return null;
        var check = Addressables.CheckForCatalogUpdates();
        check.Completed += CheckUpdateCompleted;
    }

    private void CheckUpdateCompleted(AsyncOperationHandle<List<string>> handle)
    {
        if(handle.Result.Count > 0)
        {
           Addressables.UpdateCatalogs(true, handle.Result).Completed += (updateResult) =>
            {
                onFinishedCallback?.Invoke();
            };
        }
        else
        {
            onFinishedCallback?.Invoke();
        }
    }
}
