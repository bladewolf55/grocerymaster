Write-Host "Loading functions"

function Write-Message {
    param ([String] $Message = "")
    Write-Host $Message -ForegroundColor Green 
}

# Run a command and terminate on any kind of error
function Run ([ScriptBlock]$Command, [switch]$NoExpand) {
    try {
        $Error.Clear()
        # Use this or $global:LASTEXITCODE = 0, but not $LASTEXITCODE = 0
        # otherwise Run() doesn't work properly
        Remove-Variable LASTEXITCODE -Scope global -ErrorAction Ignore
        $ErrorActionPreference = 'Stop'
        $start = Get-Date
        if (-Not($NoExpand)) {
            $msg = $ExecutionContext.InvokeCommand.ExpandString($Command)
        } 
        Write-Message $msg
        Invoke-Command -ScriptBlock $Command
        $end = Get-Date
        $elapsed = [Math]::Round(($end - $start).TotalMinutes, 3)
        Write-Message "  Command duration $elapsed minutes"
    }
    catch {}
    finally {
        if ($Error.Length -gt 0 -Or ($LASTEXITCODE -ne 0 -And -Not([string]::IsNullOrWhiteSpace($LASTEXITCODE)))) {
            # First error is last
            $err = $Error[$Error.Exception.Length - 1]
            $err = $err.InvocationInfo.PositionMessage + "`n" + $err
            # Trigger Powershell terminating error
            Write-Host $err -ForegroundColor Red
            throw "Error occurred, see above"
        }        
    }
}

function GitClean {
    # Don't remove build/dist/etc to support multiple local builds
    # Should only be run if no untracked files
    if (git ls-files --exclude-standard --others) {
        Write-Host "WARNING: You appear to have untracked files. git clean will NOT be run."
    }
    else {
        Run { git clean -d -x -q -f -e package -e dist -e .vs -e .vscode -e appsetting*.json }
        # force removing any remaining node_modules
        Run { Get-ChildItem -Filter node_modules -Recurse -Force | Remove-Item -Recurse -Force }
    }
}