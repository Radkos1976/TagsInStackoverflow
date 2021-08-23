import React, { Component } from 'react';

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { forecasts: [], loading: true };
  }

  componentDidMount() {
    this.populateWeatherData();
  }

  static renderForecastsTable(forecasts) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
                <tr>
                    <th>Tag_name</th>
                    <th>Count</th>
                    <th>Perc_Of_Probe</th>
                </tr>
        </thead>
            <tbody>
                {forecasts.map(forecast =>
                    <tr key={forecast.count}>
                        <td>{forecast.name}</td>
                        <td>{forecast.count}</td>
                        <td>{forecast.percentage} %</td>
                    </tr>
                )}
            </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchData.renderForecastsTable(this.state.forecasts);

    return (
      <div>
        <h1 id="tabelLabel" >Weather forecast</h1>
        <p>List First 10000 Tags sorted by popularity</p>
        {contents}
      </div>
    );
  }

  async populateWeatherData() {
      const response = await fetch('ValuesControllerSOF');
    const data = await response.json();
    this.setState({ forecasts: data, loading: false });
  }
}
