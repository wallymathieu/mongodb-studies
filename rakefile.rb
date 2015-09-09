require 'albacore'
require 'nuget_helper'

dir = File.dirname(__FILE__)
sln = File.join(dir, "mongodb-studies.sln")

desc "build using msbuild"
build :build do |msb|
  msb.prop :configuration, :Debug
  msb.target = [:Rebuild]
  msb.logging = 'minimal'
  msb.sln = sln
end

desc "Install missing NuGet packages."
task :restore do
  NugetHelper.exec("restore #{sln}")
end

desc "test using console"
test_runner :test => [:build] do |runner|
  runner.exe = NugetHelper.nunit_path
  files = Dir.glob(File.join(dir, "**", "bin", "Debug", "Tests.dll"))
  runner.files = files 
end

