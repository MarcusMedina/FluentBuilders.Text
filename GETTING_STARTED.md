# Getting Started - FluentBuilders.Text

Complete guide from zero to published NuGet package with trusted publishing.

---

## 📋 Step 1: Create GitHub Repository

### Option A: Via GitHub Web UI

1. **Go to GitHub**
   - Navigate to: https://github.com/new
   - Repository name: `FluentBuilders.Text`
   - Description: `Fluent string extensions for .NET with semantic namespaces`
   - Visibility: **Public**
   - ❌ **DO NOT** initialize with README (we already have one)
   - Click **Create repository**

2. **Copy the repository URL**
   ```
   https://github.com/MarcusMedina/FluentBuilders.Text.git
   ```

### Option B: Via GitHub CLI

```bash
cd /mnt/c/Git/MarcusMedina/PackageProjects/FluentBuilders.Text

gh repo create FluentBuilders.Text \
  --public \
  --description "Fluent string extensions for .NET with semantic namespaces" \
  --source . \
  --remote origin
```

---

## 📦 Step 2: Initial Git Setup & Push

```bash
cd /mnt/c/Git/MarcusMedina/PackageProjects/FluentBuilders.Text

# Initialize git (if not already initialized)
git init

# Add all files
git add .

# Create initial commit
git commit -m "Initial commit - FluentBuilders.Text v1.0.0

- Complete string extension library with semantic namespaces
- 95%+ test coverage
- Enterprise-grade quality standards
- Multi-platform CI/CD ready
- Trusted publishing configured"

# Add remote (replace with your actual repo URL)
git remote add origin https://github.com/MarcusMedina/FluentBuilders.Text.git

# Push to GitHub
git push -u origin main
```

**Expected output:**
```
Enumerating objects: XXX, done.
Counting objects: 100% (XXX/XXX), done.
...
To https://github.com/MarcusMedina/FluentBuilders.Text.git
 * [new branch]      main -> main
Branch 'main' set up to track remote branch 'main' from 'origin'.
```

---

## 🏗️ Step 3: Verify GitHub Actions

1. **Go to Actions tab**
   - Navigate to: `https://github.com/MarcusMedina/FluentBuilders.Text/actions`

2. **Watch CI workflows run**
   - ✅ **CI/CD - C#** should execute automatically
   - ✅ Build and tests should pass
   - ✅ Multi-platform testing (Ubuntu, Windows, macOS)

3. **Fix any issues**
   - If tests fail, check the logs
   - Fix issues locally
   - Push again: `git add . && git commit -m "Fix tests" && git push`

---

## 📦 Step 4: First NuGet Package (Manual)

### Build the Package Locally

```bash
cd /mnt/c/Git/MarcusMedina/PackageProjects/FluentBuilders.Text/csharp

# Restore dependencies
dotnet restore

# Build in Release mode
dotnet build --configuration Release

# Run tests to be sure
dotnet test --configuration Release

# Create NuGet package
dotnet pack --configuration Release --output ./nupkgs

# Verify package contents
ls -la ./nupkgs/
# Should see: MarcusMedina.Fluent.Text.1.0.0.nupkg
```

### Option A: Publish with API Key (Traditional)

1. **Create API Key on NuGet.org**
   - Go to: https://www.nuget.org/account/apikeys
   - Click **Create**
   - Key Name: `FluentBuilders.Text (Temporary for initial publish)`
   - Glob Pattern: `MarcusMedina.Fluent.Text`
   - Scopes: **Push new packages and package versions**
   - Expiration: 90 days
   - Click **Create**
   - **Copy the API key** (shown only once!)

2. **Publish Package**
   ```bash
   cd /mnt/c/Git/MarcusMedina/PackageProjects/FluentBuilders.Text/csharp/nupkgs

   dotnet nuget push MarcusMedina.Fluent.Text.1.0.0.nupkg \
     --api-key YOUR_API_KEY_HERE \
     --source https://api.nuget.org/v3/index.json
   ```

3. **Verify on NuGet.org**
   - Wait 5-10 minutes for indexing
   - Go to: https://www.nuget.org/packages/MarcusMedina.Fluent.Text
   - Verify version 1.0.0 is live

### Option B: Skip Manual Publish (Use Trusted Publishing from Start)

If you want to go straight to automated publishing:
- Skip manual publish
- Configure trusted publishing first (see Step 5)
- Create a release tag to trigger automated publish

---

## 🔐 Step 5: Setup Trusted Publishing

**Important:** You need at least one version published to NuGet.org before configuring trusted publishing.

### Configure NuGet.org

1. **Login to NuGet.org**
   - Go to: https://www.nuget.org
   - Sign in with your account

2. **Navigate to Package**
   - Click username → **Manage Packages**
   - Find `MarcusMedina.Fluent.Text`
   - Click **Manage**

3. **Add Trusted Publisher**
   - Click **Trusted Publishers** tab
   - Click **Add** or **Register a trusted publisher**

4. **Fill in Details**
   ```
   Publisher Type:     GitHub Actions
   Repository Owner:   MarcusMedina
   Repository Name:    FluentBuilders.Text
   Workflow File:      release.yml
   Environment:        production
   ```

5. **Save**
   - Click **Add** or **Register**
   - Verify it appears in the list

### Create GitHub Environment

1. **Go to Repository Settings**
   - Navigate to: `https://github.com/MarcusMedina/FluentBuilders.Text/settings/environments`

2. **Create Environment**
   - Click **New environment**
   - Name: `production`
   - Click **Configure environment**

3. **Add Protection Rules (Recommended)**
   - ✅ **Deployment branches**: `main` or tags matching `v*`
   - ✅ **Required reviewers**: Add yourself (optional)
   - Click **Save protection rules**

---

## 🚀 Step 6: Test Automated Publishing

### Create a Release

```bash
cd /mnt/c/Git/MarcusMedina/PackageProjects/FluentBuilders.Text

# Update version in .csproj
# Change <Version>1.0.0</Version> to <Version>1.0.1</Version>

# Update CHANGELOG.md (if exists)
# Add release notes for 1.0.1

# Commit changes
git add .
git commit -m "Release v1.0.1"

# Create and push tag
git tag v1.0.1
git push origin main
git push origin v1.0.1
```

### Monitor the Release Workflow

1. **Go to Actions**
   - Navigate to: `https://github.com/MarcusMedina/FluentBuilders.Text/actions`

2. **Watch Release Workflow**
   - Click on the **Release** workflow
   - Watch all jobs execute:
     - ✅ Validate Release
     - ✅ Build & Test Release
     - ✅ Create NuGet Package
     - ✅ Publish to NuGet (Trusted Publishing!)
     - ✅ Create GitHub Release

3. **Approve if needed**
   - If environment protection is enabled:
   - Click **Review deployments**
   - Check `production`
   - Click **Approve and deploy**

4. **Verify Success**
   - All jobs should be green ✅
   - Check NuGet.org for version 1.0.1
   - Check GitHub Releases for new release

---

## 🗑️ Step 7: Clean Up API Key (Optional)

Once trusted publishing works, remove the old API key:

1. **NuGet.org**
   - Go to: https://www.nuget.org/account/apikeys
   - Find the temporary key
   - Click **Delete**

2. **GitHub Secrets (if you added one)**
   - Go to: `https://github.com/MarcusMedina/FluentBuilders.Text/settings/secrets/actions`
   - Remove `NUGET_API_KEY` if it exists

---

## 📋 Quick Checklist

### Initial Setup
- [ ] GitHub repository created
- [ ] Initial push to GitHub completed
- [ ] CI workflow passing

### First Publish
- [ ] Package built locally and tested
- [ ] Published to NuGet.org (v1.0.0)
- [ ] Package visible on NuGet.org

### Trusted Publishing Setup
- [ ] Trusted publisher configured on NuGet.org
- [ ] GitHub `production` environment created
- [ ] Test release (v1.0.1) created
- [ ] Automated publish successful
- [ ] Old API key deleted

---

## 🎯 Daily Workflow (After Setup)

### For Regular Updates

```bash
# Make changes
# Update version in .csproj
# Update CHANGELOG.md

git add .
git commit -m "Add new feature X"
git push origin main
```

### For Releases

```bash
# Update version in .csproj: <Version>1.1.0</Version>
# Update CHANGELOG.md with release notes

git add .
git commit -m "Release v1.1.0"
git tag v1.1.0
git push origin main
git push origin v1.1.0

# Automated workflow takes over!
# - Builds package
# - Runs tests
# - Publishes to NuGet via trusted publishing
# - Creates GitHub release
```

---

## 🔧 Troubleshooting

### Git push fails: "Repository not found"

**Fix:**
```bash
# Verify remote URL
git remote -v

# If wrong, update it:
git remote set-url origin https://github.com/MarcusMedina/FluentBuilders.Text.git
```

### Tests fail in CI but pass locally

**Common causes:**
- Platform differences (Windows vs Linux)
- Missing dependencies
- Hardcoded paths

**Fix:** Check CI logs, add platform-specific test conditions if needed.

### Trusted publishing fails: "Forbidden"

**Fix:**
1. Verify publisher config on NuGet.org matches exactly
2. Ensure workflow file name is `release.yml`
3. Ensure environment name is `production`

---

## 📚 Resources

- [GitHub - Creating a Repository](https://docs.github.com/en/repositories/creating-and-managing-repositories/creating-a-new-repository)
- [NuGet - Publishing a Package](https://learn.microsoft.com/en-us/nuget/nuget-org/publish-a-package)
- [NuGet - Trusted Publishing](https://learn.microsoft.com/en-us/nuget/nuget-org/publish-a-package#trusted-publishing)
- [GitHub Actions - Workflows](https://docs.github.com/en/actions/using-workflows)

---

**Ready to ship! 🚀**
