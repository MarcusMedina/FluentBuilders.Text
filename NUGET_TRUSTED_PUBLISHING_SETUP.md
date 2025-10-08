# NuGet Trusted Publishing Setup Guide

**Trusted Publishing** eliminates API keys by using OpenID Connect (OIDC) for secure authentication between GitHub Actions and NuGet.org.

---

## ✅ Prerequisites

- NuGet package already published (initial version using API key)
- GitHub repository with GitHub Actions workflow configured
- Owner/maintainer access to package on NuGet.org

---

## 📋 Step-by-Step Setup

### 1. Configure NuGet.org Trusted Publisher

1. **Login to NuGet.org**
   - Go to [nuget.org](https://nuget.org)
   - Sign in with your account

2. **Navigate to Package Settings**
   - Click your username → **Manage Packages**
   - Find `MarcusMedina.Fluent.Text`
   - Click **Manage** button

3. **Add Trusted Publisher**
   - Go to **Trusted Publishers** section
   - Click **Add** or **Register a trusted publisher**

4. **Configure GitHub Actions Publisher**
   Fill in the form:
   ```
   Publisher Type:     GitHub Actions
   Repository Owner:   MarcusMedina
   Repository Name:    FluentBuilders.Text
   Workflow File:      release.yml
   Environment:        production
   ```

5. **Save Configuration**
   - Click **Add** or **Register**
   - Verify the publisher appears in the list

---

### 2. Update GitHub Actions Workflow

Already done! Your workflows now include:

```yaml
permissions:
  id-token: write
  contents: read

steps:
  - name: Publish to NuGet.org (Trusted Publishing)
    run: |
      dotnet nuget push ./packages/*.nupkg \
        --source https://api.nuget.org/v3/index.json \
        --skip-duplicate
    # No --api-key parameter needed!
```

**Files updated:**
- `.github/workflows/release.yml` ✅
- `.github/workflows/ci-csharp.yml` ✅

---

### 3. Create GitHub Environment (if not exists)

1. **Go to GitHub Repository Settings**
   - Navigate to: `https://github.com/MarcusMedina/FluentBuilders.Text/settings`

2. **Create Production Environment**
   - Click **Environments** (left sidebar)
   - Click **New environment**
   - Name: `production`
   - Click **Configure environment**

3. **Add Protection Rules (Optional but Recommended)**
   - ✅ **Required reviewers**: Add yourself for manual approval
   - ✅ **Wait timer**: Add 1 minute delay for safety
   - ✅ **Deployment branches**: Only `main` or tags matching `v*`

4. **Save Environment**
   - Click **Save protection rules**

---

### 4. Remove Old API Key (Optional but Recommended)

Once trusted publishing works, remove the API key:

1. **NuGet.org API Keys**
   - Go to: [nuget.org/account/apikeys](https://www.nuget.org/account/apikeys)
   - Find the key used for `MarcusMedina.Fluent.Text`
   - Click **Delete** or **Revoke**

2. **GitHub Secrets**
   - Go to: `https://github.com/MarcusMedina/FluentBuilders.Text/settings/secrets/actions`
   - Find `NUGET_API_KEY`
   - Click **Remove**

---

## 🚀 Test Trusted Publishing

### Create a Test Release

1. **Update Version**
   ```bash
   cd /mnt/c/Git/MarcusMedina/PackageProjects/FluentBuilders.Text/csharp/src/MarcusMedina.Fluent.Text
   # Edit MarcusMedina.Fluent.Text.csproj
   # Change <Version>1.0.0</Version> to <Version>1.0.1</Version>
   ```

2. **Commit and Tag**
   ```bash
   git add .
   git commit -m "Update version to 1.0.1"
   git tag v1.0.1
   git push origin main
   git push origin v1.0.1
   ```

3. **Monitor Workflow**
   - Go to: `https://github.com/MarcusMedina/FluentBuilders.Text/actions`
   - Watch the **Release** workflow execute
   - Verify it reaches the `publish` job
   - Check for successful NuGet push (no API key errors)

4. **Verify on NuGet.org**
   - Go to: `https://www.nuget.org/packages/MarcusMedina.Fluent.Text`
   - Confirm version 1.0.1 appears (may take 5-10 minutes to index)

---

## 🔍 Troubleshooting

### Error: "Package push failed: Forbidden"

**Cause:** Trusted publisher not configured correctly on NuGet.org

**Fix:**
1. Verify publisher settings on NuGet.org
2. Ensure repository owner/name match exactly
3. Ensure workflow filename is `release.yml` (not `.github/workflows/release.yml`)
4. Ensure environment name is `production` (case-sensitive)

---

### Error: "id-token permission required"

**Cause:** Workflow missing OIDC permissions

**Fix:** Ensure workflow includes:
```yaml
permissions:
  id-token: write
  contents: read
```

---

### Error: "Environment protection rules failed"

**Cause:** GitHub environment requires approval

**Fix:**
1. Go to GitHub Actions → Workflow run
2. Click **Review deployments**
3. Select `production` environment
4. Click **Approve and deploy**

---

## 📚 Additional Resources

- [NuGet Trusted Publishing Documentation](https://learn.microsoft.com/en-us/nuget/nuget-org/publish-a-package#trusted-publishing)
- [GitHub Actions OIDC](https://docs.github.com/en/actions/deployment/security-hardening-your-deployments/about-security-hardening-with-openid-connect)
- [NuGet Package Publishing](https://learn.microsoft.com/en-us/nuget/nuget-org/publish-a-package)

---

## ✅ Benefits of Trusted Publishing

1. **No API Keys** - No secrets to rotate or manage
2. **Enhanced Security** - Short-lived tokens, scoped to specific workflows
3. **Audit Trail** - NuGet.org shows which GitHub workflow published each version
4. **Zero Configuration** - No secrets to add to GitHub repository
5. **Future-Proof** - Microsoft's recommended approach for CI/CD

---

## 🎯 Quick Checklist

- [ ] Package published to NuGet.org (initial version)
- [ ] Trusted publisher configured on NuGet.org
- [ ] GitHub workflows updated with `permissions: id-token: write`
- [ ] GitHub `production` environment created
- [ ] Test release created and pushed successfully
- [ ] Old API key removed from NuGet.org
- [ ] `NUGET_API_KEY` secret removed from GitHub

---

**Ready to publish!** 🚀

Next release will use trusted publishing automatically when you push a new tag matching `v*`.
