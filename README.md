Hamper Core is the foundation of the Hamper ecosystem.

It provides the basic C# code modules that form the capability-oriented runtime and composition model that allows independent modules to discover, expose and consume functionality without compile-time dependencies. Filesystem interaction, REST- and web service communication and the composable data model create a shared understanding of the system and uses it to resolve capabilities, compose modules and execute data-driven pipelines

---

## Build

The component requires:

- .NET SDK
- C# compiler

The component can be built using either [Hamper](https://github.com/SchroedingerEntertainment/Hamper)
 or a manual build process. Hamper resolves the required packages, composes the required code modules and executes the configured build targets automatically.

If you build the modules manually, you have to perform the required steps for every module:

- resolving dependencies
- assembling code modules
- compiling the component
- generating the build output

---

## Contributing

Contributions should be made through a fork of the development branch.

Before submitting a pull request:

- ensure the change compiles, is complete and tested
- provide a clear description of the modification
- explain the motivation and expected impact of the change

Pull requests are reviewed by the project maintainers. Changes require approval from a maintainer before being merged.
A contribution must comply to the legal terms and conditions of this project.

A change missing the [Contribution Relicensing Exception](LICENSE.exception) might be rejected

---

## License

This project is licensed under the terms of the GNU Affero General Public License v3.0.

Permissions of this license are conditioned on making available complete source code of licensed works and modifications, which include larger works using a licensed work, under the same license. Copyright and license notices must be preserved. Contributors provide an express grant of patent rights. When a modified version is used to provide a service over a network, the complete source code of the modified version must be made available.

By contributing to this repository, you agree that your contributions are provided under the same license terms. Additional permissions and exceptions may apply as described in the project license and exception documents