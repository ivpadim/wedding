<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="Wedding.CloudService" generation="1" functional="0" release="0" Id="6faf6d83-e1dc-4ddc-a966-dfc469d718c9" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="Wedding.CloudServiceGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="Wedding.Mvc:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/LB:Wedding.Mvc:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Wedding.Mvc:?IsSimulationEnvironment?" defaultValue="">
          <maps>
            <mapMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/MapWedding.Mvc:?IsSimulationEnvironment?" />
          </maps>
        </aCS>
        <aCS name="Wedding.Mvc:?RoleHostDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/MapWedding.Mvc:?RoleHostDebugger?" />
          </maps>
        </aCS>
        <aCS name="Wedding.Mvc:?StartupTaskDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/MapWedding.Mvc:?StartupTaskDebugger?" />
          </maps>
        </aCS>
        <aCS name="Wedding.Mvc:AdminEmail" defaultValue="">
          <maps>
            <mapMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/MapWedding.Mvc:AdminEmail" />
          </maps>
        </aCS>
        <aCS name="Wedding.Mvc:AdminFirstName" defaultValue="">
          <maps>
            <mapMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/MapWedding.Mvc:AdminFirstName" />
          </maps>
        </aCS>
        <aCS name="Wedding.Mvc:AdminLastName" defaultValue="">
          <maps>
            <mapMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/MapWedding.Mvc:AdminLastName" />
          </maps>
        </aCS>
        <aCS name="Wedding.Mvc:AdminPassword" defaultValue="">
          <maps>
            <mapMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/MapWedding.Mvc:AdminPassword" />
          </maps>
        </aCS>
        <aCS name="Wedding.Mvc:DataConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/MapWedding.Mvc:DataConnectionString" />
          </maps>
        </aCS>
        <aCS name="Wedding.Mvc:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/MapWedding.Mvc:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="Wedding.MvcInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/MapWedding.MvcInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:Wedding.Mvc:Endpoint1">
          <toPorts>
            <inPortMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/Wedding.Mvc/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapWedding.Mvc:?IsSimulationEnvironment?" kind="Identity">
          <setting>
            <aCSMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/Wedding.Mvc/?IsSimulationEnvironment?" />
          </setting>
        </map>
        <map name="MapWedding.Mvc:?RoleHostDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/Wedding.Mvc/?RoleHostDebugger?" />
          </setting>
        </map>
        <map name="MapWedding.Mvc:?StartupTaskDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/Wedding.Mvc/?StartupTaskDebugger?" />
          </setting>
        </map>
        <map name="MapWedding.Mvc:AdminEmail" kind="Identity">
          <setting>
            <aCSMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/Wedding.Mvc/AdminEmail" />
          </setting>
        </map>
        <map name="MapWedding.Mvc:AdminFirstName" kind="Identity">
          <setting>
            <aCSMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/Wedding.Mvc/AdminFirstName" />
          </setting>
        </map>
        <map name="MapWedding.Mvc:AdminLastName" kind="Identity">
          <setting>
            <aCSMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/Wedding.Mvc/AdminLastName" />
          </setting>
        </map>
        <map name="MapWedding.Mvc:AdminPassword" kind="Identity">
          <setting>
            <aCSMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/Wedding.Mvc/AdminPassword" />
          </setting>
        </map>
        <map name="MapWedding.Mvc:DataConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/Wedding.Mvc/DataConnectionString" />
          </setting>
        </map>
        <map name="MapWedding.Mvc:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/Wedding.Mvc/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapWedding.MvcInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/Wedding.MvcInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="Wedding.Mvc" generation="1" functional="0" release="0" software="C:\Proyectos\Wedding\Wedding.CloudService\bin\Debug\Wedding.CloudService.csx\roles\Wedding.Mvc" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="?IsSimulationEnvironment?" defaultValue="" />
              <aCS name="?RoleHostDebugger?" defaultValue="" />
              <aCS name="?StartupTaskDebugger?" defaultValue="" />
              <aCS name="AdminEmail" defaultValue="" />
              <aCS name="AdminFirstName" defaultValue="" />
              <aCS name="AdminLastName" defaultValue="" />
              <aCS name="AdminPassword" defaultValue="" />
              <aCS name="DataConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;Wedding.Mvc&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;Wedding.Mvc&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/Wedding.MvcInstances" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyID name="Wedding.MvcInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="db4b1310-6b2c-43be-a6fc-794392196ff8" ref="Microsoft.RedDog.Contract\ServiceContract\Wedding.CloudServiceContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="2bc71bb7-eb76-4fee-a963-116d5f0d3d7a" ref="Microsoft.RedDog.Contract\Interface\Wedding.Mvc:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/Wedding.CloudService/Wedding.CloudServiceGroup/Wedding.Mvc:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>